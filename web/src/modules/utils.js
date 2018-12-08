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
  }
}