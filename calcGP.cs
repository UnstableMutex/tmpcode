    public enum ClaimType
    {
        Order, Claim
    }
    class sumpercent
    {
        public decimal Sum { get; set; }
        public decimal Percent { get; set; }
    }
    public class ClaimInfo
    {
        public ClaimType Type { get; set; }
        public bool HasUsualGP { get; set; }
        public bool HasNonAssetGP { get; set; }
    }
    public class GP
    {
        private const decimal NonAssetGP = 4000;
        private const decimal MinGP = 400;
        private const decimal MaxGP = 60000;
        private static sumpercent[] _arr;
        static GP()
        {
            _arr = new sumpercent[5];
            _arr[0] = new sumpercent {Sum = 20000, Percent = .04M};
            _arr[1] = new sumpercent {Sum = 100000, Percent = .03M};
            _arr[2] = new sumpercent {Sum = 200000, Percent = .02M};
            _arr[3] = new sumpercent {Sum = 1000000, Percent = .01M};
            _arr[4] = new sumpercent {Sum = decimal.MaxValue, Percent = .005M};
            TransformArr();
        }
        private static void TransformArr()
        {
            _arr = _arr.OrderBy(x => x.Sum).ToArray();
            List<sumpercent> l = new List<sumpercent> {_arr[0]};
            for (int i = 1; i <= _arr.GetUpperBound(0); i++)
            {
                l.Add(new sumpercent {Sum = _arr[i].Sum - _arr[i - 1].Sum, Percent = _arr[i].Percent});
            }
            _arr = l.ToArray();
        }
        public static decimal GetGP(decimal sum, ClaimInfo claimInfo)
        {
            decimal gp = 0;
            if (claimInfo.HasUsualGP)
            {
                gp = GetGPImpl(sum);
                switch (claimInfo.Type)
                {
                    case ClaimType.Claim:
                        break;
                    case ClaimType.Order:
                        gp /= 2;
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
            if (claimInfo.HasNonAssetGP)
            {
                gp += NonAssetGP;
            }
            return gp;
        }
        private static decimal GetGPImpl(decimal sum)
        {
            decimal gp = 0;
            foreach (var item in _arr)
            {
                gp += GetGPImpl(sum, item);
                if (sum > item.Sum)
                {
                    sum -= item.Sum;
                }
                else
                {
                    break;
                }
            }
            if (gp < MinGP)
            {
                gp = MinGP;
                return gp;
            }
            if (gp > MaxGP)
            {
                gp = MaxGP;
                return gp;
            }
            return gp;
        }
        private static decimal GetGPImpl(decimal sum, sumpercent sp)
        {
            if (sum <= sp.Sum)
            {
                return sum*sp.Percent;
            }
            else
            {
                return sp.Sum*sp.Percent;
            }
        }
    }
