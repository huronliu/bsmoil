import cache from "@/cache";
import axios from "axios";
import i18n from "@/i18n";

const TITLE_KEY = ["Mr", "Mrs", "Ms", "Madam", "Miss"];
const PREF_COMM_METHOD_KEY = ["Mail", "Phone", "Email", "Text", "No"];
const PREF_PHONE_ABBREV = ["Home", "Mobile", "None"];

function DirectEnrollCache() {
  this.cacheLocal = null;
  this.titles = null;
  this.preferredCommunicationMethods = null;
  this.preferredPhones = null;
  this.statesMap = {};
}

DirectEnrollCache.prototype.getTitleList = function() {
  if (!this.titles) {
    this.titles = cache.getObj("titles");
  }
  if (this.titles) {
    this.titles.forEach(element => {
      let key = element.Title.replace(".", "");
      if (TITLE_KEY.indexOf(key) !== -1) {
        element.Title = i18n.t(`title.${key}`);
      }
    });
  }
  return this.titles;
};
DirectEnrollCache.prototype.getRedLanternCountryList = function() {
  return cache.getObj("redLanternCountries");
};
DirectEnrollCache.prototype.getPreferredCommunicationMethodList = function() {
  if (!this.preferredCommunicationMethods) {
    this.preferredCommunicationMethods = cache.getObj(
      "preferredCommunicationMethods"
    );
  }
  if (this.preferredCommunicationMethods) {
    this.preferredCommunicationMethods.forEach(element => {
      let key = element.PreferredTypeDescription.split(" ")[0];
      if (PREF_COMM_METHOD_KEY.indexOf(key) !== -1) {
        element.PreferredTypeDescription = i18n.t(
          `preferredCommunicationMethod.${key}`
        );
      }
    });
  }
  return this.preferredCommunicationMethods;
};
DirectEnrollCache.prototype.getPreferredPhones = function() {
  if (!this.preferredPhones) {
    const preferredPhones = [];
    for (const prefPhone of PREF_PHONE_ABBREV) {
      preferredPhones.push({
        Description: i18n.t(`preferredPhone.${prefPhone}`),
        Abbrev: prefPhone
      });
    }

    this.preferredPhones = preferredPhones;
  }
  return this.preferredPhones;
};
DirectEnrollCache.prototype.getLanguageList = function() {
  return cache.getObj("languages");
};
DirectEnrollCache.prototype.getCountryList = function() {
  return cache.getObj("countries");
};
DirectEnrollCache.prototype.getStateList = function() {
  return cache.getObj("states");
};
DirectEnrollCache.prototype.getStatesOfCountry = function(countryId) {
  if (!this.statesMap.hasOwnProperty(countryId)) {
    const allStates = cache.getObj("states");
    const states = [];
    for (const state of allStates) {
      if (state.countryId === countryId) {
        states.push(state);
      }
    }

    this.statesMap[countryId] = states;
  }

  return this.statesMap[countryId];
};

DirectEnrollCache.prototype.initialize = function() {
  cache.setDataProvider("titles", () =>
    axios
      .get("/api/mdb/v1_0/advapi/settings/playertitlelist", { data: {} })
      .then(res => res.data)
  );
  cache.setDataProvider("redLanternCountries", () =>
    axios
      .get("/api/mdb/v1_0/advapi/redlanternquerycountry", { data: {} })
      .then(res => res.data)
  );
  cache.setDataProvider("preferredCommunicationMethods", () =>
    axios
      .get("/api/mdb/v1_0/advapi/preferredmethodofcommunication/list", {
        data: {}
      })
      .then(res => res.data.PreferredMethodOfCommunicationList)
  );
  cache.setDataProvider("languages", () =>
    axios
      .get("/api/mdb/v1_0/advapi/settings/languagelist", { data: {} })
      .then(res => res.data)
  );
  cache.setDataProvider("countries", () =>
    axios.get("/api/country/all", { data: {} }).then(res => res.data)
  );
  cache.setDataProvider("states", () =>
    axios.get("/api/state/all", { data: {} }).then(res => res.data)
  );

  if (this.cacheLocal != i18n.locale) {
    this.countries = null;
    this.states = null;
    this.cacheLocal = i18n.locale;
  }

  return cache.initialize([
    "titles",
    "redLanternCountries",
    "preferredCommunicationMethods",
    "languages",
    "countries",
    "states"
  ]);
};

export default new DirectEnrollCache();
