import Vue from "vue";
import Vuex from "vuex";
import decode from "jwt-decode";
import i18n from "@/i18n";
import axios from "axios";

Vue.use(Vuex);

function isTokenValid(token) {
  const jwt = decode(token);
  var dateNow = new Date();
  if (jwt.exp && jwt.exp > dateNow.getTime() / 1000) {
    return true;
  }
  return false;
}

function getObj(storage, key, defaultValue) {
  const str = storage.getItem(key);
  return str ? JSON.parse(str) : defaultValue;
}
function getBoolean(storage, key, defaultValue) {
  const str = storage.getItem(key);
  return str ? str === "true" : defaultValue;
}

function Session(w) {
  this.user = getObj(w.sessionStorage, "user", null);
  this._token = w.sessionStorage.getItem("token");
}
Object.defineProperties(Session.prototype, {
  token: {
    get: function() {
      return this._token;
    },
    set: function(token) {
      this._token = token;
      if (token) {
        axios.defaults.headers.common["Authorization"] = "Bearer " + token;
      }
    }
  }
});
Session.prototype.getUserId = function() {
  return this.user.userId;
};
Session.prototype.getRoles = function() {
  return this.user.roles;
};
Session.prototype.getLoginName = function() {
  return this.user.loginname;
};
Session.prototype.getFirstName = function() {
  return this.user.firstname;
};
Session.prototype.getLastName = function() {
  return this.user.lastname;
};
Session.prototype.isActive = function() {
  return !!this.user && !!this.token && isTokenValid(this.token);
};
Session.prototype.isAdmin = function() {
  return this.user && this.user.admin;
};
Session.prototype.getUser = function() {
  return this.user;
};
Session.prototype.setUser = function(user, token) {
  window.sessionStorage.setItem("user", JSON.stringify(user));
  window.sessionStorage.setItem("token", token);
  this.user = user;
  this.token = token;
};
Session.prototype.reload = function() {
  this.token = this._token;
  return this;
};

function ServerConfig(w) {
  this.w = w;
  this._hostUrl = null;
  this._language = null;
}
Object.defineProperties(ServerConfig.prototype, {
  hostUrl: {
    get: function() {
      return this._hostUrl;
    },
    set: function(hostUrl) {
      this._hostUrl = hostUrl;
      if (hostUrl) {
        axios.defaults.baseURL = hostUrl;
      }
    }
  }
});
Object.defineProperties(ServerConfig.prototype, {
  language: {
    get: function() {
      return this._language;
    },
    set: function(language) {
      var localeChanged = false;
      const oldLocale = i18n.locale;
      if (language) {
        this._language = language;
        if (i18n.locale !== language.value) {
          i18n.locale = language.value;
          localeChanged = true;
        }
      } else {
        if (i18n.locale == null) {
          i18n.locale = "en_US";
          localeChanged = true;
        }
      }

      if (localeChanged && Vue.prototype.$bus) {
        Vue.prototype.$bus.$emit("locale-changed", {
          newValue: i18n.locale,
          oldValue: oldLocale
        });
      }
    }
  }
});
ServerConfig.prototype.getHostUrl = function() {
  return this.hostUrl || window.location.origin;
};
ServerConfig.prototype.getLanguage = function() {
  return this.language;
};
ServerConfig.prototype.isDebugMode = function() {
  return this.debugMode;
};
ServerConfig.prototype.reload = function(data) {
  if (data) {
    if (data.hostUrl) {
      this.w.localStorage.setItem("HostUrl", data.hostUrl);
    }
    if (data.language) {
      this.w.localStorage.setItem("Language", JSON.stringify(data.language));
    }
    if (data.debugMode) {
      this.w.localStorage.setItem("DebugMode", data.debugMode);
    }
  }
  this.hostUrl = this.w.localStorage.getItem("HostUrl");
  this.language = getObj(this.w.localStorage, "Language", {
    name: "English",
    value: "en_US"
  });
  this.debugMode = getBoolean(this.w.localStorage, "DebugMode", false);
  return this;
};

export default new Vuex.Store({
  state: {
    session: new Session(window).reload(),
    settings: new ServerConfig(window).reload()
  },
  mutations: {
    setSession(state, data) {
      state.session.setUser(data.user, data.token);
    },
    reloadServerSetting(state, data) {
      state.settings.reload(data);
    }
  }
});
