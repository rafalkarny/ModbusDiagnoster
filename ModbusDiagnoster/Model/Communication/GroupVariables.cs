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
        
        
        
        
        public static List<List<HoldingRegistersVariable>> GroupHoldingRegisters(ObservableCollection<HoldingRegistersVariable> inputCollection)
            {

            List<List<HoldingRegistersVariable>> Groups = new List<List<HoldingRegistersVariable>>();

            //Sorting
            IEnumerable<HoldingRegistersVariable> sortedInput =inputCollection.OrderBy(hr => hr.StartAddress);  //Example from docs: IEnumerable<Pet> query = pets.OrderBy(pet => pet.Age);

            HoldingRegistersVariable previousVar=new HoldingRegistersVariable("", 65535);   //6535 register propably will be never used
            int PreviousVariableListIndex = -1;

            foreach(HoldingRegistersVariable hr in sortedInput)
            {
                if(PreviousVariableListIndex!=-1)
                {
                    //For Variables containing 2 words
                    if ((hr.VariableTypeFormat == "BigEndianFloat" || hr.VariableTypeFormat == "LittleEndianFloat") && (previousVar.VariableTypeFormat == "BigEndianFloat" || previousVar.VariableTypeFormat == "LittleEndianFloat"))
                    {
                        if(previousVar.StartAddress+2==hr.StartAddress)
                        {
                            if(Groups[PreviousVariableListIndex].Count()<62)   //because max requested register is 125 
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
