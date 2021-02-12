using brainflow;
using OpenCV.Net;
using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bonsai.BrainFlow
{
    [Combinator(MethodName = nameof(Generate))]
    [WorkflowElementCategory(ElementCategory.Source)]
    public abstract class BrainFlowCapture
    {
        readonly IObservable<Mat> source;
        readonly object captureLock = new object();

        public BrainFlowCapture()
        {
            source = Observable.Create<Mat>((observer, cancellationToken) =>
            {
                return Task.Factory.StartNew(() =>
                {
                    var bufferLength = BufferLength;
                    var inputParams = new BrainFlowInputParams();
                    var boardId = (int)ConfigureBoard(inputParams);
                    var channelCount = BoardShim.get_num_rows(boardId);
                    var sampleRate = BoardShim.get_sampling_rate(boardId);
                    var bufferSize = (int)Math.Ceiling(sampleRate * bufferLength / 1000.0);
                    var captureInterval = TimeSpan.FromMilliseconds((int)(bufferLength / 2 + 0.5));
                    var captureBufferSize = bufferSize * 100;

                    lock (captureLock)
                    {
                        BoardShim.enable_dev_board_logger();
                        var boardShim = new BoardShim(boardId, inputParams);
                        var input_json = inputParams.to_json();
                        boardShim.prepare_session();
                        try
                        {
                            var readBuffer = new double[channelCount * bufferSize];
                            boardShim.start_stream(captureBufferSize);
                            using (var captureSignal = new ManualResetEvent(false))
                            {
                                while (!cancellationToken.IsCancellationRequested)
                                {
                                    while (boardShim.get_board_data_count() >= bufferSize)
                                    {
                                        var error = BoardControllerLibrary.get_board_data(bufferSize, readBuffer, boardId, input_json);
                                        if (error != (int)CustomExitCodes.STATUS_OK)
                                        {
                                            throw new BrainFlowException(error);
                                        }

                                        var buffer = Mat.FromArray(readBuffer, channelCount, bufferSize, Depth.F64, 1);
                                        observer.OnNext(buffer);
                                    }

                                    captureSignal.WaitOne(captureInterval);
                                }
                            }
                        }
                        finally
                        {
                            boardShim.stop_stream();
                            boardShim.release_session();
                        }
                    }
                },
                cancellationToken,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
            })
            .PublishReconnectable()
            .RefCount();
        }

        [Description("The length of the streaming buffer, in milliseconds.")]
        public int BufferLength { get; set; }

        internal abstract BoardIds ConfigureBoard(BrainFlowInputParams inputParams);

        public IObservable<Mat> Generate()
        {
            return source;
        }
    }
}
