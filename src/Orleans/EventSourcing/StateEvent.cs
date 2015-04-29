/*
Project Orleans Cloud Service SDK ver. 1.0
 
Copyright (c) Microsoft Corporation
 
All rights reserved.
 
MIT License

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and 
associated documentation files (the ""Software""), to deal in the Software without restriction,
including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS
OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;


namespace Orleans.EventSourcing
{
    /// <summary>
    /// Basic low level container for events
    /// </summary>
    public class StateEvent
    {
        /// <summary>
        /// ID of  event unique withing the scope of the grain
        /// </summary>
        public string Id { get; private set; }
        /// <summary>
        /// Application-specific type of event.
        /// </summary>
        public string EventType { get; private set; }
        /// <summary>
        /// Extra data for event. Only makes sense for untyped events. It's ugly that we expose it for StateEvent&lt;T&gt;
        /// </summary>
        public object EventData { get; private set; }
        /// <summary>
        /// Time when event was raised.
        /// </summary>
        public DateTime Timestamp { get; private set; }
        /// <summary>
        /// Correlation ID for associating event with a request or transaction.
        /// </summary>
        public string CorrelationId { get; private set; }
        /// <summary>
        /// Additional context, such as user/device ID, security token, etc.
        /// </summary>
        public string Context { get; private set; }

        public StateEvent(string id, string eventType, DateTime timestamp, object eventData=null, string correlationId=null, string context=null)
        {
            Id = id;
            EventType = eventType;
            EventData = eventData;
            Timestamp = timestamp;
            CorrelationId = correlationId;
            Context = context;
        }

        public StateEvent(string id, string eventType, object eventData=null)
            : this(id, eventType, DateTime.UtcNow, eventData)
        {
        }

        public virtual void Apply(IGrainState aggregateState)
        {
        }
    }

    /// <summary>
    /// Abstract base class for events that can apply themselves onto an aggregate grain state
    /// </summary>
    /// <typeparam name="T">Grain state interface  event can be applied to.</typeparam>
    public abstract class StateEvent<T> : StateEvent
        where T : IGrainState
    {
        protected StateEvent(string id, string eventType, DateTime timestamp, string correlationId = null, string context = null)
            : base(id, eventType, timestamp, correlationId, context)
        {
        }

        protected StateEvent(string id, string eventType)
            : this(id, eventType, DateTime.UtcNow)
        {
        }

        public abstract void Apply(T aggregateState);
        public override void Apply(IGrainState aggregateState)
        {
            Apply((T) aggregateState);
        }
    }
}