namespace BasicTaxFreeAllowanceApi.Models
{
    public class BTFARecord
    {
        private static long Records = 0;

        private Double _Income;
        public Double Income
        {
            get
            {
                return _Income;
            }
            set
            {
                _Income = value;
                Btfa = calculateBtfa(_Income);
            }
        }
        public string Id { get; private set; }

        public Double Btfa { get; private set; }


        public BTFARecord()
        {
            System.Threading.Interlocked.Increment(ref Records);
            Id = "BESK2022_" + Records;
        }

        private Double calculateBtfa(Double income)
        {
            Double Result;
            const Double PBB = 48_300;

            // Round down to nearest 100
            income = Math.Round(income / 100, MidpointRounding.ToNegativeInfinity) * 100;

            switch (income)
            {
                case <= 0.99 * PBB:
                    Result = 0.423 * PBB;
                    break;
                case <= 2.72 * PBB:
                    Result = 0.423 * PBB + 0.2 * (income - 0.99 * PBB);
                    break;
                case <= 3.11 * PBB:
                    Result = 0.77 * PBB;
                    break;
                case <= 7.88 * PBB:
                    Result = 0.77 * PBB - 0.1 * (income - 3.11 * PBB);
                    break;
                case > 7.88 * PBB:
                    Result = 0.293 * PBB;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Result = Math.Round(Result / 100, MidpointRounding.ToPositiveInfinity) * 100;
            Result = Math.Min(income, Result);

            return Result;
        }
    }
}
