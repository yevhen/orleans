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
    public class PersonRegistered : StateEvent<IPersonState>
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public GenderType Gender { get; private set; }

        public PersonRegistered()
            : base(Guid.NewGuid().ToString(), "JournaledPerson.PersonRegistered")
        {
        }

        public PersonRegistered(string firstName, string lastName, GenderType gender)
            : this()
        {
            FirstName = firstName;
            LastName = lastName;
            Gender = gender;
        }

        public override void Apply(IPersonState state)
        {
            state.FirstName = FirstName;
            state.LastName = LastName;
            state.Gender = Gender;
        }
    }

    public class PersonMarried : StateEvent<IPersonState>
    {
        public Guid SpouseId { get; set; }
        public string SpouseFirstName { get; set; }
        public string SpouseLastName { get; set; }


        public PersonMarried()
            : base(Guid.NewGuid().ToString(), "JournaledPerson.PersonMarried")        {
            
        }
        public PersonMarried(Guid spouseId, string spouseFirstName, string spouseLastName)
            : this()
        {
            SpouseId = spouseId;
            SpouseFirstName = spouseFirstName;
            SpouseLastName = spouseLastName;
        }

        public override void Apply(IPersonState state)
        {
            // TODO
        }
    }

    public class PersonLastNameChanged : StateEvent<IPersonState>
    {
        public string LastName { get; set; }

        public PersonLastNameChanged()
            : base(Guid.NewGuid().ToString(), "JournaledPerson.PersonRegistered")
        {
        }

        public PersonLastNameChanged(string lastName)
            : this()
        {
            LastName = lastName;
        }

        public override void Apply(IPersonState state)
        {
            state.LastName = LastName;
        }
    }
}