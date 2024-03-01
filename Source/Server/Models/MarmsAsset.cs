using System.Collections;
using System.Collections.Generic;

namespace WDS.Models
{
    public class MarmsAsset
    {
        #region Column Map
        /// <summary>
        /// The type of asset (per categories such as Real Property Inventory (RPI) Codes, 
        /// UFC 4-020-01, table 3-1, facility type, Military Intelligence Database (MIDB) Equipment Codes, 
        /// DHS Infrastructure Data Taxonomy (IDT) which uses North American Industry Classification System (NAICS) Codes, etc.), 
        /// relevant to the MA program. The type of asset could be non-physical infrastructure, 
        /// for example logical software application types (e.g., compiled software vs embedded firmware), information types.
        /// Current Enumeration allows for: (DCI, Cyber, Other)
        /// 
        /// Example: DCI
        /// </summary>
        public IEnumerable<IList> AssetCategory { get; set; }      // Req: yes, Char max: 10000, List: yes
        /// <summary>
        /// Name of asset (as determined by asset owner). The formal, descriptive title by which 
        /// an asset is designated and distinguished from others. 
        /// This title should preferably not contain abbreviations or acronyms.
        /// 
        /// Example: Radar Alpha
        /// </summary>
        public string AssetName { get; set; }                       // Req: yes, Char max: 10000, List: no
        /// <summary>
        /// Detailed but concise description of asset (classification determined by level of detail). 
        /// Should include an explanation of the nature and characteristics of an asset, 
        /// such as its generic functionality or capabilities, 
        /// and not include details of criticality levels of support to any one specific military mission.
        /// 
        /// Example: This is a radar for detecting incoming objects
        /// </summary>
        public string AssetDescription { get; set; }                // Req: yes, Char max: 100000, List: no
        /// <summary>
        /// A designator indicating the current operational status of the subject asset.
        /// Valid enumerations: 1. Functioning Normally; 
        ///                     2. Degraded - Maintenance; 
        ///                     3. Degraded - Emergency; 
        ///                     4 - Destroyed; 
        ///                     5 - Inactive
        ///                     
        /// Example:  2. Degraded -Maintenance
        /// </summary>
        public IEnumerable<IList> AssetOpStatus { get; set; }       // Req: no, Char max: n/a, List: yes
        /// <summary>
        /// The peak critical power of an asset. Units are in MW.
        /// Eample 18: MW
        /// </summary>
        public string PeakCriticalPowerMW { get; set; }             // Req: yes, char max: 100, list no
        /// <summary>
        /// The total energy consumption of an asset. Units are in MWh.
        /// Example: 29 MWh
        /// </summary>
        public string TotalEnergyConsumptionMWh { get; set; }       // Req: yes, char max: 100, list no
        /// <summary>
        /// A list of capabilities that the Mission Assurance Asset provides.
        /// Example:  [“Detect”, “Identify”, "Destroy"]
        /// </summary>
        public string[] AssetCapabilities { get; set; }             // req: no, Max entries: 100, char max: 1000
        /// <summary>
        /// This should read "Asset Owner"
        /// 
        /// Example: Asset Owner
        /// </summary>
        public string AOPocType { get; set; }               // req: no, char max: 1000 
        /// <summary>
        /// Point of contact (POC) name, as either an Organization name, or an individual
        /// person's First Name and Last Name
        /// 
        /// Example: John Smith
        /// </summary>
        public string AOPocName { get; set; }               // req: no, char max: 1000
        /// <summary>
        /// This is the Unit Identification Code
        /// 
        /// Exampe: 123456
        /// </summary>
        public string AOPocUIC { get; set; }                // req: no, char max: 100
        /// <summary>
        /// The formal, descriptive name by which a POC Organization is designated and distinguished from others. 
        /// This name should, preferably, not contain abbreviations or acronyms.
        /// 
        /// Example:  US ARMY
        /// </summary>
        public string AOPocOrganizationName { get; set; }  // req: no, char max: 100
        /// <summary>
        /// The formal first name of the individual POC
        /// 
        /// Example: John
        /// </summary>
        public string AOPocFirstName { get; set; }         // req: no, char max: 500
        /// <summary>
        /// The formal last name of the individual POC.
        /// 
        /// Example: Smith
        /// </summary>
        public string AOPocLastName { get; set; }          // req: no, char max: 500
        /// <summary>
        /// E-mail address of POC for item of interest.
        /// 
        /// Example: john.doe@live.com
        /// </summary>
        public IEnumerable<string> AOPocEmailAddress { get; set; }      // req: no, max entries: 100, char max: 100
        /// <summary>
        /// Telephone number of POC for item of interest (including country code, if outside United States)
        /// 
        /// Example:  973-555-5555
        /// </summary>
        public IEnumerable<string> AOPocTelephoneNumber { get; set; }  // req: no, max entries: 100, char max: 100
        /// <summary>
        /// Indicates whether the Asset is fixed, movable, or mobile. Current enumeration allows: (Fixed, Movable, Mobile)
        /// 
        /// Example: Fixed
        /// </summary>
        public IEnumerable<string> LocationType { get; set; } // req: no, max entries: 100, char max: n/a
        /// <summary>
        /// Street address of physical location of the Asset.
        /// 
        /// Example: 123 Adams Street
        /// </summary>
        public string LocStreetAddress1 { get; set; }  // req: no, char max: 1000
        /// <summary>
        /// Secondary physical address of the Asset (e.g., Bldg. #, Unit #, Suite #, Pier #).
        /// </summary>
        public string LocStreetAddress2 { get; set; }  // req: no, char max: 1000
        /// <summary>
        /// The name of the city or municipal area in which the Asset is currently located or its normal stowage location.
        /// 
        /// Example: Long Island City
        /// </summary>
        public string LocCity { get; set; }  // req: no, char max: 1000
        /// <summary>
        /// Postal Code corresponding to physical address of the Asset (5 digits)
        /// 
        /// Example: 12345
        /// </summary>
        public string LocPostalCode { get; set; }  // req: no, char max: 100
        /// <summary>
        /// International Standards Organization 3166 Country Code (3-character trigraph) indicating 
        /// country in which the Asset is physically located or homebased or home-ported.
        /// 
        /// Example: USA
        /// </summary>
        public string LocCountryCode { get; set; } // req: no, char max: 100
        /// <summary>
        /// A designator indicating the military installation on which the Asset is located, if any. 
        /// User should reference the Real Property Inventory (RPI) Installation list for common naming 
        /// as the authoritative list of installations.
        /// 
        /// Example: Picatinny Arsenal
        /// </summary>
        public string LocInstallation { get; set; } // req: no, char max: 1000
        /// <summary>
        /// A code designator indicating the military installation on which the Asset is located, if any. 
        /// User should reference the Real Property Inventory (RPI) Installation list for common naming as 
        /// the authoritative list of installations. This would contain either the Site or Installation Code.
        /// 
        /// Example: 120NG
        /// </summary>
        public string LocRPICode { get; set; } // req: no, char max: 1000
        /// <summary>
        /// The designation or number of the building or facility in which the Asset is currently located or its normal stowage location.
        /// 
        /// Example: 4
        /// </summary>
        public string LocBuildingNumber { get; set; }   //req: no, char max: 100
        /// <summary>
        /// The designation or number of the floor or level of the building on which the Asset 
        /// is currently located or its normal stowage location. 
        /// (This is typically not a part of the mailing address.)
        /// 
        /// Example: 1
        /// </summary>
        public string LocFloor { get; set; }  // req: no, char max: 100
        /// <summary>
        /// The designation or number of the area, such as a room number, wing, of the building in which 
        /// the Asset is currently located or its normal stowage location. (This is typically not a part of the mailing address.)
        /// 
        /// Example: 1201A
        /// </summary>
        public string LocRoom { get; set; }  // req: no, char max: 100
        /// <summary>
        /// The facility population in terms of Building, Room, and Floor where Asset is located. (e.g., 1000; n/a; 10). 
        /// (If only Building Number was provided, Floor & Room would be ‘n/a’) Field is used for AT/FP purposes 
        /// during Criticality Assessment (AT Standard 5) to identify mass gathering facilities or 
        /// high-occupancy buildings. This field is only relevant for AT assessment purposes where 
        /// facility categorization is required per DoDI 2000.12.
        /// Note: This should be blank for Asset location.
        /// </summary>
        public string LocPopulation { get; set; } // req: no, char max: 100
        /// <summary>
        /// Longitude coordinates of the Asset in decimal degrees (minimum precision of 4 decimal places, e.g., -90.1713); 
        /// negative for west of prime meridian. For movable or a mobile items of interest where it is currently based or controlled
        /// 
        /// Example: -90.1713
        /// </summary>
        public string LocLongitude { get; set; } // req: no, char max: N/A
        /// <summary>
        /// Latitude coordinates of the Asset in decimal degrees (minimum precision of 4 decimal places, e.g., 42.1252); 
        /// negative for south of equator. 
        /// For movable or a mobile items of interest where it is currently based or controlled.
        /// 
        /// Example: 42.1252
        /// </summary>
        public string LocLatitude { get; set; } // req: no, char max: N/A
        /// <summary>
        /// Method used to obtain lat-long coordinates (e.g., geocode/address match, global positioning system, 
        /// map/imagery interpretation, feature extraction, ancillary authoritative data source, or other source)
        /// 
        /// Example: Ancillary
        /// </summary>
        public string LocCoordSourceClass { get; set; }  // req: no, char max: 500
        /// <summary>
        /// Role for POC organization or individual: Asset Owner, Mission Owner, 
        /// Program Management POC, responsible CNDSP/CSSP POC 
        /// (Computer Network Defense Service Provider/Cyber Security Service Provider), OPR, etc.
        /// 
        /// Example: Program Management POC
        /// </summary>
        public string AddPocType { get; set; }               // req: no, char max: 1000 
        /// <summary>
        /// Point of contact (POC) name, as either an Organization name, or an individual
        /// person's First Name and Last Name
        /// 
        /// Example: John Smith
        /// </summary>
        public string AddPocName { get; set; }               // req: no, char max: 1000
        /// <summary>
        /// This is the Unit Identification Code
        /// 
        /// Exampe: 123456
        /// </summary>
        public string AddPocUIC { get; set; }                // req: no, char max: 100
        /// <summary>
        /// The formal, descriptive name by which a POC Organization is designated and distinguished from others. 
        /// This name should, preferably, not contain abbreviations or acronyms.
        /// 
        /// Example:  US ARMY
        /// </summary>
        public string AddPocOrganizationName { get; set; }  // req: no, char max: 100
        /// <summary>
        /// The formal first name of the individual POC
        /// 
        /// Example: John
        /// </summary>
        public string AddPocFirstName { get; set; }         // req: no, char max: 500
        /// <summary>
        /// The formal last name of the individual POC.
        /// 
        /// Example: Smith
        /// </summary>
        public string AddPocLastName { get; set; }          // req: no, char max: 500
        /// <summary>
        /// E-mail address of POC for item of interest.
        /// 
        /// Example: john.doe@live.com
        /// </summary>
        public IEnumerable<string> AddPocEmailAddress { get; set; }      // req: no, max entries: 100, char max: 100
        /// <summary>
        /// Telephone number of POC for item of interest (including country code, if outside United States)
        /// 
        /// Example:  973-555-5555
        /// </summary>
        public IEnumerable<string> AddPocTelephoneNumber { get; set; }  // req: no, max entries: 100, char max: 100

        #endregion
    }
}
