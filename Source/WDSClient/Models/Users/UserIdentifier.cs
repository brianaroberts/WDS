namespace WDSClient.Models.Users
{
    public class UserIdentifier
    {
        public const int EDIPI_LENGTH = 10;
        private int _edipi;
        private string _ptc = string.Empty;
        private string _code; 

        protected static readonly Dictionary<string, string> _EdipiCodeMap = new Dictionary<string, string>()
        {
            { "A", "MIL" },
            { "J", "MIL" },
            { "N", "MIL" },
            { "V", "MIL" },
            { "B", "CIV" },
            { "C", "CIV" },
            { "I", "CIV" },
            { "E", "CTR" },
            { "O", "CTR" },
            { "T", "FM" },
            { "U", "FN" },
            { "K", "NAF" },
            { "M", "NGO" },
            { "R", "RET" },
            { "F", "VET" },
        };

        public int EDIPI 
        { 
            get
            {
                return _edipi;
            } 
            set
            {
                _edipi = value; 
            }
        }
        public string PTC 
        {
            get
            {
                return _ptc; 
            }
            set
            {
                if (value.Length == 1)
                {
                    _ptc = value;
                }
            }
        }
        public string Code
        {
            get
            {
                return _code; 
            }
        }
        public bool IsValid { get; set; }
        public string ADUserName { get; set; }
        public string OriginalValue { get; set; }

        public UserIdentifier(string possibleValue)
        {
            IsValid = false;
            var tryValue = OriginalValue = possibleValue.Trim(); 

            int periodPosition = possibleValue.IndexOf(".");

            if (periodPosition > 0)  // 10.A 10.MIL
            {
                var pieces = possibleValue.Split(".");
                if (pieces[0].Length == EDIPI_LENGTH)
                {
                    tryValue = pieces[0];
                    if (pieces[1].Length == 1)
                    {
                        _ptc = pieces[1];
                        if (_EdipiCodeMap.ContainsKey(_ptc)) _code = _EdipiCodeMap[_ptc]; 
                    }
                    if (pieces[1].Length == 3)
                        _code = pieces[1]; 
                }
            }

            IsValid = int.TryParse(tryValue, out _edipi);

            if (!IsValid)
            {
                // Assume this is an AD User Identifier
                ADUserName = possibleValue; 
            }
        }
    }
}
