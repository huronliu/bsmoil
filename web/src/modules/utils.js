import Vue from 'vue';

export default {
  showLoading() {
    Vue.prototype.$bus.$emit("show-loading");
  },
  hideLoading() {
    Vue.prototype.$bus.$emit("hide-loading");
  },
  toast(text, timeout) {
    Vue.prototype.$bus.$emit("toast", { text, timeout });
  },
  getBaseUrl() {
    var baseurl = "/";
    if (process.env.BASE_URL) {
      baseurl = process.env.BASE_URL;
    } else {
      if (window.$isAndroid) {
        baseurl = '/android_asset/www/';
      } else {
        baseurl = "/";
      }
    }
    return baseurl;
  }
}