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
using System.Threading.Tasks;
using Orleans;
using Orleans.EventSourcing;
using TestGrainInterfaces;

namespace TestGrains
{
    /* This is a total speculation. I'm not sure we need or can make this work.
     * This was inspired by the elegance of @ReubenBond's implementayion of ES.
     * It is quite possible this is a total nonsense. I only put it here for a discusssion.
     * The alternative is to build a boilerplate codegen on top of the typed events,
     * and not to have untyped events in the API at all.
    */
    public class LambdaJournaledPersonGrain : JournaledGrain<IPersonState>, IJournaledPersonGrain
    {
        public Task RegisterBirth(PersonAttributes props)
        {
            return RaiseStateEvent(GenerateEvent("LambdaJournaledPersonGrain.RegisterBirth", 
                props.FirstName, props.LastName, props.Gender), 
                ApplyEvents);
        }

        public async Task Marry(IJournaledPersonGrain spouse)
        {
            var spouseData = await spouse.GetPersonalAttributes();

            await RaiseStateEvent(GenerateEvent("LambdaJournaledPersonGrain.Marry", 
                spouse.GetPrimaryKey(), spouseData.FirstName, spouseData.LastName),
                ApplyEvents,
                commit: false); // We are not storing the first event here


            if (State.LastName != spouseData.LastName)
            {
                await RaiseStateEvent(GenerateEvent("LambdaJournaledPersonGrain.LastNameChanged", 
                    spouseData.LastName),
                    ApplyEvents,
                    commit: false);
            }

            await State.WriteStateAsync();
        }

        public Task<PersonAttributes> GetPersonalAttributes()
        {
            return Task.FromResult(new PersonAttributes
            {
                FirstName = State.FirstName,
                LastName = State.LastName,
                Gender = State.Gender
            });
        }

        private static StateEvent GenerateEvent(string eventType, params object[] args)
        {
            return new StateEvent(Guid.NewGuid().ToString(), eventType, DateTime.UtcNow, args);
        }

        private static void ApplyEvents(IPersonState state, StateEvent @event)
        {
            // TODO
        }
    }
}