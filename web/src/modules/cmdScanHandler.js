import moment from "moment";

function CMBHandler() {}
CMBHandler.prototype.handleScanDLResult = function(codeNumber) {
  var returnObj = {};
  var objDataElement = {};

  var dataElement = [
    //2D Mandatory data elements
    "DCA",
    "DCB",
    "DCD",
    "DBA",
    "DCS",
    "DAC",
    "DAD",
    "DBD",
    "DBB",
    "DBC",
    "DAY",
    "DAU",
    "DAG",
    "DAI",
    "DAJ",
    "DAK",
    "DAQ",
    "DCF",
    "DCG",
    "DDE",
    "DDF",
    "DDG",
    //Custom DL data elements
    "DL",
    "ZN",
    "DCT",
    "DCH",
    "DAA",
    "DAR",
    "DAB",
    "DAL",
    "DAO",
    "DAP",
    "DAM",
    "DAN",
    //Optional data elements
    "DAH",
    "DAZ",
    "DCI",
    "DCJ",
    "DCK",
    "DBN",
    "DBG",
    "DBS",
    "DCU",
    "DCE",
    "DCL",
    "DCM",
    "DCN",
    "DCO",
    "DCP",
    "DCQ",
    "DCR",
    "DDA",
    "DDB",
    "DDC",
    "DDD",
    "DAW",
    "DAX",
    "DDH",
    "DDI",
    "DDJ",
    "DDK",
    "DDL",
    //Additional data elements
    "DAS",
    "DAT",
    "DBH",
    "DBK"
  ];

  for (var i = 0; i < dataElement.length; i++) {
    var nIndex = codeNumber.indexOf(dataElement[i]);
    if (nIndex == -1) continue;

    var parts = codeNumber.substring(nIndex + 3);
    var endIndex = parts.indexOf("\n");
    if (endIndex == -1) continue;
    objDataElement[dataElement[i]] = parts.substring(0, endIndex).trim();
    console.log(dataElement[i] + "===" + objDataElement[dataElement[i]]);
  }

  //LastName DCS
  if (objDataElement.DCS != undefined && objDataElement.DCS != "") {
    console.log("lastname == " + objDataElement.DCS);
    returnObj.lastName = objDataElement.DCS;
  } else {
    //LastName DAB
    if (objDataElement.DAB != undefined && objDataElement.DAB) {
      console.log("lastname == " + objDataElement.DAB);
      returnObj.lastName = objDataElement.DAB;
    }
  }

  //FirstName MiddleName, for NV, GA, IA, VA, WA,
  if (objDataElement.DCT != undefined && objDataElement.DCT != "") {
    var tmpFN = objDataElement.DCT;
    var nSpaceIdx = tmpFN.indexOf(" ");
    if (nSpaceIdx != -1) {
      returnObj.firstName = tmpFN.substring(0, nSpaceIdx);
      returnObj.middleName = tmpFN.substring(nSpaceIdx + 1);
    } else {
      var nComIdx = tmpFN.indexOf(",");
      if (nComIdx != -1) {
        returnObj.firstName = tmpFN.substring(0, nComIdx);
        returnObj.middleName = tmpFN.substring(nComIdx + 1);
      } else returnObj.firstName = tmpFN;
    }
  }

  //FirstName DAC
  if (objDataElement.DAC != undefined && objDataElement.DAC != "") {
    returnObj.firstName = objDataElement.DAC;
  }

  //MiddleName DAD
  if (
    objDataElement.DAD != undefined &&
    objDataElement.DAD != "" &&
    objDataElement.DAD != "NONE"
  ) {
    returnObj.middleName = objDataElement.DAD;
  }

  //LASTNAME,FIRSTNAME,MIDDLENAME,
  if (objDataElement.DAA != undefined && objDataElement.DAA) {
    var strName = objDataElement.DAA;
    if (strName.indexOf(" ") > -1) {
      var allName = strName.split(" ");
      if (allName[0]) {
        returnObj.firstName = allName[0];
      }
      if (allName[2]) {
        returnObj.lastName = allName[2];
        if (allName[1]) {
          returnObj.middleName = allName[1];
        }
      } else if (allName[1]) {
        returnObj.lastName = allName[1];
      }
    } else {
      if (strName.substring(strName.length - 1) == ",") {
        strName = strName.substring(0, strName.length - 1);
      }

      var arrayName = strName.split(",");
      returnObj.lastName = arrayName[0];
      returnObj.firstName = arrayName[1];
      if (arrayName[2] === undefined || arrayName[2] === "") {
        returnObj.middleName = "";
      } else {
        returnObj.MiddleName = arrayName[2];
      }
    }
  }

  //Driver's License Number
  if (objDataElement.DAQ != undefined && objDataElement.DAQ) {
    returnObj.idNumber = objDataElement.DAQ;
  }

  //Expired Date
  if (objDataElement.DBA != undefined && objDataElement.DBA) {
    returnObj.idExpireDate = formatTime(objDataElement.DBA);
  }

  //DOB
  if (objDataElement.DBB != undefined && objDataElement.DBB) {
    returnObj.birthdate = formatTime(objDataElement.DBB);
  }

  //Gender
  if (objDataElement.DBC != undefined && objDataElement.DBC) {
    if (!isNaN(parseInt(objDataElement.DBC))) {
      if (objDataElement.DBC == "1") returnObj.gender = "M";
      else returnObj.gender = "F";
    } else returnObj.gender = objDataElement.DBC;
  }

  //Gender MD
  //This is for Maryland DL since DDA is used as Gender field, while DBC is set to 1
  if (
    objDataElement.DAJ != undefined &&
    objDataElement.DAJ != "" &&
    objDataElement.DAJ == "MD"
  )
    returnObj.gender = objDataElement.DDA;

  //State
  if (objDataElement.DAJ != undefined && objDataElement.DAJ != "")
    returnObj.idState = objDataElement.DAJ;
  else {
    if (objDataElement.DAO != undefined && objDataElement.DAO != "")
      returnObj.idState = objDataElement.DAO;
  }

  //HomeAddress
  if (objDataElement.DAG != undefined && objDataElement.DAG != "")
    returnObj.homeAddress = objDataElement.DAG;
  else {
    if (objDataElement.DAL != undefined && objDataElement.DAL != "")
      returnObj.homeAddress = objDataElement.DAL;
  }

  //AddressCity
  if (objDataElement.DAI != undefined && objDataElement.DAI != "")
    returnObj.homeCity = objDataElement.DAI;
  else {
    if (objDataElement.DAN != undefined && objDataElement.DAN != "")
      returnObj.homeCity = objDataElement.DAN;
  }

  //AddressState
  if (objDataElement.DAJ != undefined && objDataElement.DAJ != "")
    returnObj.homeState = objDataElement.DAJ;
  else {
    if (objDataElement.DAO != undefined && objDataElement.DAO != "")
      returnObj.homeState = objDataElement.DAO;
  }

  //Zipcode
  var strZipCode;
  if (objDataElement.DAK != undefined && objDataElement.DAK != "") {
    strZipCode = objDataElement.DAK;
    if (strZipCode.length > 5) strZipCode = strZipCode.substring(0, 5);
    returnObj.homePostalCode = strZipCode;
  } else {
    if (objDataElement.DAP != undefined && objDataElement.DAP != "") {
      strZipCode = objDataElement.DAP;
      if (strZipCode.length > 5) strZipCode = strZipCode.substring(0, 5);
      returnObj.homePostalCode = strZipCode;
    }
  }
  return returnObj;
};

function formatTime(sourceTime) {
  if (moment(sourceTime, "MMDDYYYY").isValid()) {
    return moment(sourceTime, "MMDDYYYY").format("YYYY-MM-DD");
  }
  if (moment(sourceTime, "YYYYMMDD").isValid()) {
    return moment(sourceTime, "YYYYMMDD").format("YYYY-MM-DD");
  }
  return null;
}

export default new CMBHandler();
