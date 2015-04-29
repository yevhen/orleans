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
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Orleans.EventSourcing
{
    public class JournaledGrain<TGrainState> : Grain<TGrainState>
        where TGrainState : class, IJournaledGrainState
    {
        protected delegate void ApplyAction(TGrainState state, StateEvent @event);

        /// <summary>
        /// This methiod is for events that know how to apply themselves to TGrainState, subclasses of StateEvent&lt;T&gt;.
        /// </summary>
        /// <param name="event">Event to raise</param>
        /// <param name="commit">Whether or not the event needs to be immediately committed to storage</param>
        /// <returns></returns>
        protected Task RaiseStateEvent(StateEvent<TGrainState> @event, bool commit = true)
        {
            return RaiseStateEventImpl(@event, null, commit);
        }

        /// <summary>
        /// This one is totally speculative and likely nonsense. Just an attampt to explore if we can raise untyped events 
        /// without defining a hierarchy of event classes. A single subclass of of StateEvent&lt;T&gt; for multiple event types
        /// may be a better and more pragmatic option. Still leaving it here for a discusssion.
        /// </summary>
        /// <param name="event">Event to raise</param>
        /// <param name="apply">Delegate for a method that can apply this event to TGrainState</param>
        /// <param name="commit">Whether or not the event needs to be immediately committed to storage</param>
        /// <returns></returns>
        protected Task RaiseStateEvent(StateEvent @event, ApplyAction apply, bool commit = true)
        {
            return RaiseStateEventImpl(@event, apply, commit);
        }

        /// <summary>
        /// This method is only necessary if we support the untyped version of RaiseStateEvent with an apply delegate.
        /// </summary>
        /// <param name="event"></param>
        /// <param name="apply"></param>
        /// <param name="commit"></param>
        /// <returns></returns>
        private Task RaiseStateEventImpl(StateEvent @event, ApplyAction apply, bool commit = true)
        {
            if (@event == null) throw new ArgumentNullException("event");

            // State.AddEvent(@event);
            // switch to AddEvent after updating codegen for state classes
            State.UncommittedEvents.Add(@event);

            if (@event.GetType().IsAssignableFrom(typeof (StateEvent))) // untyped event
            {
                if (apply == null) throw new ArgumentNullException("apply");

                // TODO: verify that apply can be serialized and recreated at replay time
                // TODO: append serialized apply as EventData
                apply(State, @event);
            }
            else
            {
                @event.Apply(State);
            }

            return commit
                 ? State.WriteStateAsync()
                 : TaskDone.Done;
        }
    }
}