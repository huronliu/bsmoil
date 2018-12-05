import "material-design-icons-iconfont/dist/material-design-icons.css"; // Ensure you are using css-loader
import "@fortawesome/fontawesome-free/css/all.min.css";
import "@babel/polyfill";
import Vue from "vue";
import "@/plugins/vuetify";
import App from "@/App.vue";
import router from "@/router";
import store from "@/store";
import axios from "axios";
import _ from "lodash";
import cache from "@/cache";
import globalConfig from "@/modules/globalConfig";
import MobileHeader from "@/components/MobileHeader";
import MobileFooter from "@/components/MobileFooter";

Vue.component("mobile-header", MobileHeader);
Vue.component("mobile-footer", MobileFooter);

const eventBus = new Vue();
Object.defineProperties(Vue.prototype, {
  $bus: {
    get: function() {
      return eventBus;
    }
  }
});
Vue.prototype.$utils = {
  showLoading() {
    Vue.prototype.$bus.$emit("show-loading");
  },
  hideLoading() {
    Vue.prototype.$bus.$emit("hide-loading");
  },
  toast(text, timeout) {
    Vue.prototype.$bus.$emit("toast", { text, timeout });
  }
};

axios.defaults.headers.common["Content-Type"] = "application/json";
Vue.prototype.$http = axios;
Vue.prototype.$cache = cache;
Vue.prototype.$config = {
  getObj(key) {
    let configs = Vue.prototype.$cache.getObj("configs");
    if (configs) {
      return configs[key];
    }

    return null;
  },
  get(key, defaultValue) {
    let config = this.getObj(key);
    if (config) {
      return config.value !== undefined ? config.value : defaultValue;
    }

    return defaultValue;
  }
};

Vue.config.productionTip = false;

axios.interceptors.response.use(
  response => response,
  error => {
    if (error && error.response && error.response.status) {
      if (error.response.status === 401) {
        console.error(
          "You are not authenticated",
          JSON.stringify(error.response)
        );
        if (store.state.session.isActive()) {
          console.log("should go to log out");
          //common.logout()
          // TODO:: should we logout the user and redirect to the /login page???
        }
      } else if (error.response.status === 403) {
        console.error("You are not authorized", JSON.stringify(error.response));
      }
    }
    return Promise.reject(error);
  }
);

router.beforeEach((to, from, next) => {
  console.debug("router change: %s --> %s", from.path, to.path);
  // Only able to receive the session and settings from home page
  // Expecting all the floowing pamarters send via the query string:
  //  1. token
  //  2. user
  //  3. hostUrl (optional, default to window.location.origin)
  //  4. language (optional, default to {value: 'en_US'})
  //  5. debugMode (optional, default to false)
  if (to.path === "/" && to.query && to.query.token) {
    const token = to.query.token;
    const user = to.query.user && JSON.parse(decodeURI(to.query.user));
    const hostUrl = to.query.hostUrl && decodeURI(to.query.hostUrl);
    const debugMode = to.query.debugMode && to.query.debugMode === "true";

    store.commit("setSession", { user, token });
    store.commit("reloadServerSetting", {
      hostUrl: hostUrl,
      debugMode: debugMode
    });
    globalConfig.clear();
  }

  // Redirect to /admin if logged-in as admin and requesting to "/"
  if (to.path === "/" && store.state.session.isAdmin()) {
    next({ path: "/admin", replace: true });
    return;
  }

  // TODO::  we will add checking of roles later
  if (_.isNil(to.meta) || !to.meta.requireAuth) {
    next();
    return;
  }

  if (to.meta.requireAuth) {
    if (store.state.session.isActive()) {
      globalConfig
        .init()
        .then(() => next())
        .catch(() => alert("Unable to initialize the global configs"));
    } else {
      console.debug("Authentication required");
      next({ path: "/login", query: null });
    }
  }
});

let appInitialized = false;
function initApp() {
  if (!appInitialized) {
    appInitialized = true;
    return new Vue({
      router,
      store,
      render: h => h(App)
    }).$mount("#app");
  }
}

if (window.$isAndroid || window.$isIOS) {
  document.addEventListener("deviceready", () => initApp(), false);
  // Always show up the web pages even cordova not loaded
  //  Give a warnning if cordova not loaded
  setTimeout(() => {
    if (!appInitialized) {
      initApp();
      alert(
        "Native plugins failed to load, you can continue to use this app, but will be unable to use the device features"
      );
    }
  }, 30 * 1000);
} else {
  initApp();
}
