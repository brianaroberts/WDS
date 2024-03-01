namespace WDS.Models.JperStat
{
    public class C2IEDailyModel
    {
        #region Map

        public DateTime EntryDate { get; set; }
        public int CommandId { get; set; }
        public string CommandName { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public byte[] GeoCode { get; set; }
        public int Army { get; set; }
        public int Navy { get; set; }
        public int AirForce { get; set; }
        public int SpaceForce { get; set; }
        public int MarineCorps { get; set; }
        public int CoastGuard { get; set; }
        public int DodCivilian { get; set; }
        public int ContractorCivilian { get; set; }
        public int OtherCivilian { get; set; }
        public int CoalitionCount { get; set; }
        public string CoalitionCountry { get; set; }
        public int BaseLine { get; set; }
        public int BootsOnGround { get; set; }
        public int ReliefInPlace { get; set; }
        public int TempEnablingForce { get; set; }
        public int OverTheHorizon { get; set; }
        public int TheaterCoordAssist { get; set; }

        #endregion Map

    }
}
