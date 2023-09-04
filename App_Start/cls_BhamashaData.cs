using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Xml.Serialization;
using System.Collections.Generic;
using FMDSS.Models;

namespace FMDSS.App_Start
{
    public class cls_BhamashaData
    {
    }


    [Serializable]
    [XmlRoot(ElementName = "member")]
    public class Member : BaseModelSerializable
    {
        [XmlElement(ElementName = "ackId")]
        public string AckId { get; set; }
        [XmlElement(ElementName = "familyId")]
        public string FamilyId { get; set; }
        [XmlElement(ElementName = "nameEng")]
        public string NameEng { get; set; }
        [XmlElement(ElementName = "nameHnd")]
        public string NameHnd { get; set; }
        [XmlElement(ElementName = "gender")]
        public string Gender { get; set; }
        [XmlElement(ElementName = "fnameEng")]
        public string FnameEng { get; set; }
        [XmlElement(ElementName = "fnameHnd")]
        public string FnameHnd { get; set; }
        [XmlElement(ElementName = "mnameEng")]
        public string MnameEng { get; set; }
        [XmlElement(ElementName = "mnameHnd")]
        public string MnameHnd { get; set; }
        [XmlElement(ElementName = "snameEng")]
        public string SnameEng { get; set; }
        [XmlElement(ElementName = "snameHnd")]
        public string SnameHnd { get; set; }
        [XmlElement(ElementName = "dob")]
        public string Dob { get; set; }
        [XmlElement(ElementName = "mid")]
        public string Mid { get; set; }
        [XmlElement(ElementName = "aadhar")]
        public string Aadhar { get; set; }
        [XmlElement(ElementName = "eid")]
        public string Eid { get; set; }
        [XmlElement(ElementName = "mobile")]
        public string Mobile { get; set; }
        [XmlElement(ElementName = "acc")]
        public string Acc { get; set; }
        [XmlElement(ElementName = "bankName")]
        public string BankName { get; set; }
        [XmlElement(ElementName = "ifsc")]
        public string Ifsc { get; set; }
        [XmlElement(ElementName = "voterId")]
        public string VoterId { get; set; }
        [XmlElement(ElementName = "panNo")]
        public string PanNo { get; set; }
        [XmlElement(ElementName = "passport")]
        public string Passport { get; set; }
        [XmlElement(ElementName = "dlNo")]
        public string DlNo { get; set; }
        [XmlElement(ElementName = "email")]
        public string Email { get; set; }
        [XmlElement(ElementName = "qualification")]
        public string Qualification { get; set; }
        [XmlElement(ElementName = "relationTyp")]
        public string RelationTyp { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "personalInfo")]
    public class PersonalInfo:BaseModelSerializable
    {
        [XmlElement(ElementName = "member")]
        public List<Member> Member { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "familydetail")]
    public class Familydetail:BaseModelSerializable
    {
        [XmlElement(ElementName = "addressHnd")]
        public string AddressHnd { get; set; }
        [XmlElement(ElementName = "addressEng")]
        public string AddressEng { get; set; }
        [XmlElement(ElementName = "caste")]
        public string Caste { get; set; }
        [XmlElement(ElementName = "category")]
        public string Category { get; set; }
        [XmlElement(ElementName = "isRural")]
        public string IsRural { get; set; }
        [XmlElement(ElementName = "districtName")]
        public string DistrictName { get; set; }
        [XmlElement(ElementName = "block_city")]
        public string Block_city { get; set; }
        [XmlElement(ElementName = "gp")]
        public string Gp { get; set; }
        [XmlElement(ElementName = "ward")]
        public string Ward { get; set; }
        [XmlElement(ElementName = "village")]
        public string Village { get; set; }
        [XmlElement(ElementName = "annIncome")]
        public string AnnIncome { get; set; }
        [XmlElement(ElementName = "landType")]
        public string LandType { get; set; }
        [XmlElement(ElementName = "landCat")]
        public string LandCat { get; set; }
        [XmlElement(ElementName = "livSince")]
        public string LivSince { get; set; }
        [XmlElement(ElementName = "gasConn")]
        public string GasConn { get; set; }
        [XmlElement(ElementName = "gasAgency")]
        public string GasAgency { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "family")]
    public class Family:BaseModelSerializable
    {
        [XmlElement(ElementName = "familydetail")]
        public Familydetail Familydetail { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "root")]
    public class BhamashaRoot:BaseModelSerializable
    {
        [XmlElement(ElementName = "requestId")]
        public string RequestId { get; set; }
        [XmlElement(ElementName = "cmsg")]
        public string Cmsg { get; set; }
        [XmlElement(ElementName = "personalInfo")]
        public PersonalInfo PersonalInfo { get; set; }
        [XmlElement(ElementName = "family")]
        public Family Family { get; set; }
    }





    //[XmlRoot(ElementName="member")]
    //public class Member {
    //    [XmlElement(ElementName="ackId")]
    //    public string AckId { get; set; }
    //    [XmlElement(ElementName="familyId")]
    //    public string FamilyId { get; set; }
    //    [XmlElement(ElementName="nameEng")]
    //    public string NameEng { get; set; }
    //    [XmlElement(ElementName="nameHnd")]
    //    public string NameHnd { get; set; }
    //    [XmlElement(ElementName="gender")]
    //    public string Gender { get; set; }
    //    [XmlElement(ElementName="fnameEng")]
    //    public string FnameEng { get; set; }
    //    [XmlElement(ElementName="fnameHnd")]
    //    public string FnameHnd { get; set; }
    //    [XmlElement(ElementName="mnameEng")]
    //    public string MnameEng { get; set; }
    //    [XmlElement(ElementName="mnameHnd")]
    //    public string MnameHnd { get; set; }
    //    [XmlElement(ElementName="snameEng")]
    //    public string SnameEng { get; set; }
    //    [XmlElement(ElementName="snameHnd")]
    //    public string SnameHnd { get; set; }
    //    [XmlElement(ElementName="dob")]
    //    public string Dob { get; set; }
    //    [XmlElement(ElementName="mid")]
    //    public string Mid { get; set; }
    //    [XmlElement(ElementName="aadhar")]
    //    public string Aadhar { get; set; }
    //    [XmlElement(ElementName="eid")]
    //    public string Eid { get; set; }
    //    [XmlElement(ElementName="mobile")]
    //    public string Mobile { get; set; }
    //    [XmlElement(ElementName="acc")]
    //    public string Acc { get; set; }
    //    [XmlElement(ElementName="bankName")]
    //    public string BankName { get; set; }
    //    [XmlElement(ElementName="ifsc")]
    //    public string Ifsc { get; set; }
    //    [XmlElement(ElementName="voterId")]
    //    public string VoterId { get; set; }
    //    [XmlElement(ElementName="panNo")]
    //    public string PanNo { get; set; }
    //    [XmlElement(ElementName="passport")]
    //    public string Passport { get; set; }
    //    [XmlElement(ElementName="dlNo")]
    //    public string DlNo { get; set; }
    //    [XmlElement(ElementName="email")]
    //    public string Email { get; set; }
    //    [XmlElement(ElementName="qualification")]
    //    public string Qualification { get; set; }
    //    [XmlElement(ElementName="relationTyp")]
    //    public string RelationTyp { get; set; }
    //}

    //[XmlRoot(ElementName="personalInfo")]
    //public class PersonalInfo {
    //    [XmlElement(ElementName="member")]
    //    public List<Member> Member { get; set; }
    //}

    //[XmlRoot(ElementName="familydetail")]
    //public class Familydetail {
    //    [XmlElement(ElementName="addressHnd")]
    //    public string AddressHnd { get; set; }
    //    [XmlElement(ElementName="addressEng")]
    //    public string AddressEng { get; set; }
    //    [XmlElement(ElementName="caste")]
    //    public string Caste { get; set; }
    //    [XmlElement(ElementName="annIncome")]
    //    public string AnnIncome { get; set; }
    //    [XmlElement(ElementName="landType")]
    //    public string LandType { get; set; }
    //    [XmlElement(ElementName="landCat")]
    //    public string LandCat { get; set; }
    //    [XmlElement(ElementName="livSince")]
    //    public string LivSince { get; set; }
    //    [XmlElement(ElementName="gasConn")]
    //    public string GasConn { get; set; }
    //    [XmlElement(ElementName="gasAgency")]
    //    public string GasAgency { get; set; }
    //}

    //[XmlRoot(ElementName="family")]
    //public class Family {
    //    [XmlElement(ElementName="familydetail")]
    //    public Familydetail Familydetail { get; set; }
    //}

    //[XmlRoot(ElementName="root")]
    //public class BhamashaRoot {
    //    [XmlElement(ElementName="requestId")]
    //    public string RequestId { get; set; }
    //    [XmlElement(ElementName="cmsg")]
    //    public string Cmsg { get; set; }
    //    [XmlElement(ElementName="personalInfo")]
    //    public PersonalInfo PersonalInfo { get; set; }
    //    [XmlElement(ElementName="family")]
    //    public Family Family { get; set; }

    //}

}
