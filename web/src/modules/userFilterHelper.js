import axios from "axios";

function UserFilterHelper() {}

var myFilter = {
  sortBy: "coinIn",
  sortDirection: "desc",
  coinMin: null,
  coinMax: null,
  carded: "null",
  hotPlayer: "null",
  sections: null,
  approach: "null",
  marketAuthorizer: null
};

UserFilterHelper.prototype.loadUserFilter = function() {
  return loadFilter();
};

UserFilterHelper.prototype.saveUserFilter = function() {
  return saveFilter();
};

UserFilterHelper.prototype.setSortBy = function(newValue) {
  myFilter.sortBy = newValue;
};

UserFilterHelper.prototype.getSortBy = function() {
  return myFilter.sortBy;
};

UserFilterHelper.prototype.setSortDirection = function(newValue) {
  myFilter.sortDirection = newValue;
};

UserFilterHelper.prototype.getSortDirection = function() {
  return myFilter.sortDirection;
};

UserFilterHelper.prototype.setSections = function(newValue) {
  myFilter.sections = newValue;
};

UserFilterHelper.prototype.getSections = function() {
  return myFilter.sections;
};

UserFilterHelper.prototype.setCoinMin = function(newValue) {
  myFilter.coinMin = newValue;
};

UserFilterHelper.prototype.getCoinMin = function() {
  return myFilter.coinMin;
};

UserFilterHelper.prototype.setCoinMax = function(newValue) {
  myFilter.coinMax = newValue;
};

UserFilterHelper.prototype.getCoinMax = function() {
  return myFilter.coinMax;
};

UserFilterHelper.prototype.setCarded = function(newValue) {
  myFilter.carded = newValue;
};

UserFilterHelper.prototype.getCarded = function() {
  return myFilter.carded;
};

UserFilterHelper.prototype.setHotPlayer = function(newValue) {
  myFilter.hotPlayer = newValue;
};

UserFilterHelper.prototype.getHotPlayer = function() {
  return myFilter.hotPlayer;
};

UserFilterHelper.prototype.setApproach = function(newValue) {
  myFilter.approach = newValue;
};

UserFilterHelper.prototype.getApproach = function() {
  return myFilter.approach;
};

UserFilterHelper.prototype.setMarketAuthorizer = function(newValue) {
  myFilter.marketAuthorizer = newValue;
};

UserFilterHelper.prototype.getMarketAuthorizer = function() {
  return myFilter.marketAuthorizer;
};

UserFilterHelper.prototype.getParameter = function() {
  var parameter = "?";
  if (hasValue(myFilter.sections)) {
    var arrayLength = myFilter.sections.length;
    if (arrayLength > 0) {
      parameter = parameter + "sections=";
      for (var i = 0; i < arrayLength; i++) {
        parameter = parameter + myFilter.sections[i];
      }
    }
  }

  if (hasValue(myFilter.coinMin)) {
    var ci = parseInt(myFilter.coinMin);
    if (ci > 0) {
      parameter = parameter + "&coinin_min=" + ci;
    }
  }

  if (hasValue(myFilter.coinMax)) {
    var co = parseInt(myFilter.coinMax);
    if (co > 0) {
      parameter = parameter + "&coinin_max=" + co;
    }
  }

  if (hasValue(myFilter.carded)) {
    parameter = parameter + "&carded=" + myFilter.carded;
  }

  if (hasValue(myFilter.approach)) {
    parameter = parameter + "&approached=" + myFilter.approach;
  }

  if (hasValue(myFilter.marketAuthorizer)) {
    parameter = parameter + "&hosted_by=" + myFilter.marketAuthorizer;
  }

  if (hasValue(myFilter.hotPlayer)) {
    parameter = parameter + "&hot_player=" + myFilter.hotPlayer;
  }

  return parameter;
};

function loadFilter() {
  var url = "/api/filter/";
  return axios.get(url).then(result => {
    if (result.data != undefined && result.data != "") {
      myFilter = result.data;
      console.log(myFilter);
    }
    return myFilter;
  });
}
function hasValue(val) {
  if (val == undefined) return false;
  if (val == "") return false;
  if (val == "null") return false;

  return true;
}

function saveFilter() {
  var settings = { settings: JSON.stringify(myFilter) };
  var url = "/api/filter/update";

  axios
    .post(url, settings)
    .then(result => {
      console.info(result);
    })
    .catch(error => {
      console.log(error.message);
    });
}

export default new UserFilterHelper();
