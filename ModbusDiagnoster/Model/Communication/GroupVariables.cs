using ModbusDiagnoster.Model.Variables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusDiagnoster.Model.Communication
{
    public class GroupVariables
    {
        //Using list becouse we not need to call event when changed
        //Docs:
        //https://docs.microsoft.com/pl-pl/dotnet/api/system.linq.enumerable.orderby?view=net-6.0#System_Linq_Enumerable_OrderBy__2_System_Collections_Generic_IEnumerable___0__System_Func___0___1__



        //COILS
        public static List<List<CoilsVariable>> GroupCoils(ObservableCollection<CoilsVariable> inputCollection)
        {

            List<List<CoilsVariable>> Groups = new List<List<CoilsVariable>>();

            //Sorting
            IEnumerable<CoilsVariable> sortedInput = inputCollection.OrderBy(di => di.StartAddress);  //Example from docs: IEnumerable<Pet> query = pets.OrderBy(pet => pet.Age);

            CoilsVariable previousVar = new CoilsVariable("", 65535);   //6535 register propably will be never used
            int PreviousVariableListIndex = -1;

            foreach (CoilsVariable di in sortedInput)
            {
                if (PreviousVariableListIndex != -1)
                {

                    //For Variables Containing 1 word 

                    if (previousVar.StartAddress + 1 == di.StartAddress)
                    {
                        if (Groups[PreviousVariableListIndex].Count() < 2000)   //because max requested registers are 251x8 is 2008
                        {
                            Groups[PreviousVariableListIndex].Add(di);  //Add variable to previous list 
                        }
                        else
                        {
                            List<CoilsVariable> tmp = new List<CoilsVariable>();  //Add new List (new group) and add variable
                            tmp.Add(di);
                            Groups.Add(tmp);
                            PreviousVariableListIndex = Groups.IndexOf(tmp);
                        }

                    }
                    else
                    {
                        List<CoilsVariable> tmp = new List<CoilsVariable>();  //Add new List (new group) and add variable
                        tmp.Add(di);
                        Groups.Add(tmp);
                        PreviousVariableListIndex = Groups.IndexOf(tmp);
                    }


                }
                else
                {
                    List<CoilsVariable> tmp = new List<CoilsVariable>();
                    tmp.Add(di);
                    Groups.Add(tmp);
                    PreviousVariableListIndex = 0;
                }
                previousVar = di;

            }
            return Groups;


        }

        // Discrete inputs
        public static List<List<DiscreteInputsVariable>> GroupDiscreteInputs(ObservableCollection<DiscreteInputsVariable> inputCollection)
        {

            List<List<DiscreteInputsVariable>> Groups = new List<List<DiscreteInputsVariable>>();

            //Sorting
            IEnumerable<DiscreteInputsVariable> sortedInput = inputCollection.OrderBy(di => di.StartAddress);  //Example from docs: IEnumerable<Pet> query = pets.OrderBy(pet => pet.Age);

            DiscreteInputsVariable previousVar = new DiscreteInputsVariable("", 65535);   //6535 register propably will be never used
            int PreviousVariableListIndex = -1;

            foreach (DiscreteInputsVariable di in sortedInput)
            {
                if (PreviousVariableListIndex != -1)
                {

                    //For Variables Containing 1 word 

                    if (previousVar.StartAddress + 1 == di.StartAddress)
                    {
                        if (Groups[PreviousVariableListIndex].Count() < 2000)   //because max requested register is 125 
                        {
                            Groups[PreviousVariableListIndex].Add(di);  //Add variable to previous list 
                        }
                        else
                        {
                            List<DiscreteInputsVariable> tmp = new List<DiscreteInputsVariable>();  //Add new List (new group) and add variable
                            tmp.Add(di);
                            Groups.Add(tmp);
                            PreviousVariableListIndex = Groups.IndexOf(tmp);
                        }

                    }
                    else
                    {
                        List<DiscreteInputsVariable> tmp = new List<DiscreteInputsVariable>();  //Add new List (new group) and add variable
                        tmp.Add(di);
                        Groups.Add(tmp);
                        PreviousVariableListIndex = Groups.IndexOf(tmp);
                    }


                }
                else
                {
                    List<DiscreteInputsVariable> tmp = new List<DiscreteInputsVariable>();
                    tmp.Add(di);
                    Groups.Add(tmp);
                    PreviousVariableListIndex = 0;
                }
                previousVar = di;

            }
            return Groups;


        }


        //INPUT REGISTERS
        public static List<List<InputRegistersVariable>> GroupInputRegisters(ObservableCollection<InputRegistersVariable> inputCollection)
        {

            List<List<InputRegistersVariable>> Groups = new List<List<InputRegistersVariable>>();

            //Sorting
            IEnumerable<InputRegistersVariable> sortedInput = inputCollection.OrderBy(hr => hr.StartAddress);  //Example from docs: IEnumerable<Pet> query = pets.OrderBy(pet => pet.Age);

            InputRegistersVariable previousVar = new InputRegistersVariable("", 65535);   //6535 register propably will be never used
            int PreviousVariableListIndex = -1;

            foreach (InputRegistersVariable ir in sortedInput)
            {
                if (PreviousVariableListIndex != -1)
                {
                    //For Variables containing 2 words
                    if ((ir.VariableTypeFormat == "BigEndianFloat" || ir.VariableTypeFormat == "LittleEndianFloat") && (previousVar.VariableTypeFormat == "BigEndianFloat" || previousVar.VariableTypeFormat == "LittleEndianFloat"))
                    {
                        if (previousVar.StartAddress + 2 == ir.StartAddress)
                        {
                            if (Groups[PreviousVariableListIndex].Count() < 62)   //because max requested register is 125 
                            {
                                Groups[PreviousVariableListIndex].Add(ir);  //Add variable to previous list 
                            }
                            else
                            {
                                List<InputRegistersVariable> tmp = new List<InputRegistersVariable>();  //Add new List (new group) and add variable
                                tmp.Add(ir);
                                Groups.Add(tmp);
                                PreviousVariableListIndex = Groups.IndexOf(tmp);
                            }

                        }
                        else
                        {
                            List<InputRegistersVariable> tmp = new List<InputRegistersVariable>();  //Add new List (new group) and add variable
                            tmp.Add(ir);
                            Groups.Add(tmp);
                            PreviousVariableListIndex = Groups.IndexOf(tmp);
                        }
                    }

                    //For Variables Containing 1 word 
                    else
                    {
                        if (previousVar.StartAddress + 1 == ir.StartAddress)
                        {
                            if (Groups[PreviousVariableListIndex].Count() < 125)   //because max requested register is 125 
                            {
                                Groups[PreviousVariableListIndex].Add(ir);  //Add variable to previous list 
                            }
                            else
                            {
                                List<InputRegistersVariable> tmp = new List<InputRegistersVariable>();  //Add new List (new group) and add variable
                                tmp.Add(ir);
                                Groups.Add(tmp);
                                PreviousVariableListIndex = Groups.IndexOf(tmp);
                            }

                        }
                        else
                        {
                            List<InputRegistersVariable> tmp = new List<InputRegistersVariable>();  //Add new List (new group) and add variable
                            tmp.Add(ir);
                            Groups.Add(tmp);
                            PreviousVariableListIndex = Groups.IndexOf(tmp);
                        }
                    }

                }
                else
                {
                    List<InputRegistersVariable> tmp = new List<InputRegistersVariable>();
                    tmp.Add(ir);
                    Groups.Add(tmp);
                    PreviousVariableListIndex = 0;
                }
                previousVar = ir;

            }




            return Groups;


        }



        //HOLDING REGISTERS
        public static List<List<HoldingRegistersVariable>> GroupHoldingRegisters(ObservableCollection<HoldingRegistersVariable> inputCollection)
        {

            List<List<HoldingRegistersVariable>> Groups = new List<List<HoldingRegistersVariable>>();

            //Sorting
            IEnumerable<HoldingRegistersVariable> sortedInput = inputCollection.OrderBy(hr => hr.StartAddress);  //Example from docs: IEnumerable<Pet> query = pets.OrderBy(pet => pet.Age);

            HoldingRegistersVariable previousVar = new HoldingRegistersVariable("", 65535);   //6535 register propably will be never used
            int PreviousVariableListIndex = -1;

            foreach (HoldingRegistersVariable hr in sortedInput)
            {
                if (PreviousVariableListIndex != -1)
                {
                    //For Variables containing 2 words
                    if ((hr.VariableTypeFormat == "BigEndianFloat" || hr.VariableTypeFormat == "LittleEndianFloat") && (previousVar.VariableTypeFormat == "BigEndianFloat" || previousVar.VariableTypeFormat == "LittleEndianFloat"))
                    {
                        if (previousVar.StartAddress + 2 == hr.StartAddress)
                        {
                            if (Groups[PreviousVariableListIndex].Count() < 62)   //because max requested register is 125 
                            {
                                Groups[PreviousVariableListIndex].Add(hr);  //Add variable to previous list 
                            }
                            else
                            {
                                List<HoldingRegistersVariable> tmp = new List<HoldingRegistersVariable>();  //Add new List (new group) and add variable
                                tmp.Add(hr);
                                Groups.Add(tmp);
                                PreviousVariableListIndex = Groups.IndexOf(tmp);
                            }

                        }
                        else
                        {
                            List<HoldingRegistersVariable> tmp = new List<HoldingRegistersVariable>();  //Add new List (new group) and add variable
                            tmp.Add(hr);
                            Groups.Add(tmp);
                            PreviousVariableListIndex = Groups.IndexOf(tmp);
                        }
                    }

                    //For Variables Containing 1 word 
                    else
                    {
                        if (previousVar.StartAddress + 1 == hr.StartAddress)
                        {
                            if (Groups[PreviousVariableListIndex].Count() < 125)   //because max requested register is 256 
                            {
                                Groups[PreviousVariableListIndex].Add(hr);  //Add variable to previous list 
                            }
                            else
                            {
                                List<HoldingRegistersVariable> tmp = new List<HoldingRegistersVariable>();  //Add new List (new group) and add variable
                                tmp.Add(hr);
                                Groups.Add(tmp);
                                PreviousVariableListIndex = Groups.IndexOf(tmp);
                            }

                        }
                        else
                        {
                            List<HoldingRegistersVariable> tmp = new List<HoldingRegistersVariable>();  //Add new List (new group) and add variable
                            tmp.Add(hr);
                            Groups.Add(tmp);
                            PreviousVariableListIndex = Groups.IndexOf(tmp);
                        }
                    }

                }
                else
                {
                    List<HoldingRegistersVariable> tmp = new List<HoldingRegistersVariable>();
                    tmp.Add(hr);
                    Groups.Add(tmp);
                    PreviousVariableListIndex = 0;
                }
                previousVar = hr;

            }




            return Groups;


        }

    }
}
