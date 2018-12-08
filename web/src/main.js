import Vue from 'vue';
//import './plugins/vuetify';
import "@/plugins/vuetify";

import App from './App.vue'
import store from './store'
import router from './router';
import Footer from './components/Footer';
import Header from './components/Header';
import 'material-design-icons-iconfont/dist/material-design-icons.css';
import utils from './modules/utils';
import axios from 'axios';

Vue.config.productionTip = false
Vue.component('mobile-footer', Footer);
Vue.component('mobile-header', Header);

const eventBus = new Vue();
Object.defineProperties(Vue.prototype, {
  $bus: {
    get: function() {
      return eventBus;
    }
  }
});
Vue.prototype.$utils = utils;

axios.defaults.headers.common["Content-Type"] = "application/json";
Vue.prototype.$http = axios;

router.beforeEach((to, from, next) => {
  console.debug(`router change: ${from.path} --> ${to.path}`);
  // Only able to receive the session and settings from home page
  // Expecting all the floowing pamarters send via the query string:
  //  1. token
  //  2. user
  //  3. hostUrl (optional, default to window.location.origin)
  //  4. language (optional, default to {value: 'en_US'})
  //  5. debugMode (optional, default to false)
  // if (to.path === "/" && to.query && to.query.token) {
  //   const token = to.query.token;
  //   const user = to.query.user && JSON.parse(decodeURI(to.query.user));
  //   const hostUrl = to.query.hostUrl && decodeURI(to.query.hostUrl);
  //   const debugMode = to.query.debugMode && to.query.debugMode === "true";

  //   store.commit("setSession", { user, token });
  //   store.commit("reloadServerSetting", {
  //     hostUrl: hostUrl,
  //     debugMode: debugMode
  //   });
  //   globalConfig.clear();
  // }
  next();
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
