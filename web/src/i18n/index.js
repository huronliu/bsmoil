import Vue from "vue";
import VueI18n from "vue-i18n";
import enUS from "@/i18n/en_US.js";
import zhCn from "@/i18n/zh_CN.js";
import zhMo from "@/i18n/zh_MO.js";

Vue.use(VueI18n);

// Ready translated locale messages
const messages = {
  en_US: enUS,
  zh_CN: zhCn,
  zh_MO: zhMo
};

// Create VueI18n instance with options
export default new VueI18n({
  locale: "en_US", // set locale
  fallbackLocale: "en_US",
  messages // set locale messages
});
