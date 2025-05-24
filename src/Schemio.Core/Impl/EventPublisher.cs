using System;
using System.Collections.Generic;

namespace Schemio.Core.Impl
{
    public class EventPublisher
    {
        private readonly ISubscriber<ExecutorResultArgs> subscriber;

        public EventPublisher(ISubscriber<ExecutorResultArgs> subscriber)
        {
            this.subscriber = subscriber;
        }

        public void PublishEvent(IDataContext context, ExecutorResultArgs args) => subscriber.OnEventHandler(context, args);
    }

    public class ExecutorResultArgs : EventArgs
    {
        public ExecutorResultArgs(IEnumerable<IQueryResult> result)
        {
            Result = result;
        }

        public IEnumerable<IQueryResult> Result { get; }
    }

    public interface ISubscriber<ExecutorResultArgs>
    {
        void OnEventHandler(IDataContext context, ExecutorResultArgs e);
    }
}