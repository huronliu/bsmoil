import Vue from "vue";
import "@/plugins/vuetify";
import router from "@/router";
import store from "@/store";
import "@/registerServiceWorker";

function logout() {
  var rootUrl = store.getters.root;
  var myName = store.getters.loginname;
  Vue.prototype.$http
    .post(rootUrl + "/auth/logout", { loginname: myName })
    .then(
      result => {
        console.info(result);
      },
      error => {
        console.info(error);
      }
    );
  localStorage.removeItem("user-token");
  Vue.prototype.$http.defaults.headers.common["Authorization"] = "";
  router.push("login");
}

export default {
  logout
};
