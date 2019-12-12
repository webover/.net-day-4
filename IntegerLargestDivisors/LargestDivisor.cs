using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace IntegerLargestDivisors
{
    class LargestDivisor
    {
        private readonly int NrOfElementsToProcess;
        private readonly int Start;
        public LargestDivisor(int start, int nrOfElementsToProcess)
        {
            this.Start = start;
            this.NrOfElementsToProcess = nrOfElementsToProcess;
        }

        public int largestDivisors { get; private set; }
        public int count { get; private set; }

        public void CalculateDivisor()
        {
            for (int j = this.Start * this.NrOfElementsToProcess; j < (this.Start + 1) * this.NrOfElementsToProcess; j++)
            {
                List<int> listDivisors = new List<int>();

                for (int x = 1; x <= j; x++)
                {
                    if (j % x == 0)
                    {
                        listDivisors.Add(x);
                    }
                }

                if (this.count < listDivisors.Count)
                {
                    this.largestDivisors = j;
                    this.count = listDivisors.Count;                    
                }
            }
        }
    }
}
