import Vue from 'vue'
import './plugins/vuetify'
import VueLocalStorage from 'vue-localstorage'
import App from './App.vue'
import store from './store'
import router from './router'
import EventBus from './modules/eventbus.js'

Vue.config.productionTip = false;
Vue.use(VueLocalStorage, {
  name: 'ls',
  bind: true
});
Vue.prototype.$bus = EventBus;
Vue.prototype.$util = {
  info(text) {
    Vue.prototype.$bus.$emit('snackbar', { text: text, type: 'info' });
  },
  error(text) {
    Vue.prototype.$bus.$emit('snackbar', { text: text, type: 'error' });
  }
}

new Vue({
  store,
  router,
  render: h => h(App)
}).$mount('#app')
