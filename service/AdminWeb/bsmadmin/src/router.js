import Vue from 'vue';
import Router from 'vue-router';
import Home from './views/Home.vue';
import Setting from './views/Setting.vue';
import Stations from './views/Stations.vue';
import Monitor from './views/Monitor.vue';
import Users from './views/Users.vue';

Vue.use(Router)

export default new Router({
  routes: [
    {
      path: '/',
      name: 'home',
      component: Home
    },
    {
      path: '/setting',
      name: 'setting',
      component: Setting
    },
    {
      path: '/stations',
      name: 'stations',
      component: Stations
    },
    {
      path: '/monitor',
      name: 'monitor',
      component: Monitor
    },
    {
      path: '/users',
      name: 'users',
      component: Users
    },
    {
      path: '/about',
      name: 'about',
      // route level code-splitting
      // this generates a separate chunk (about.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import(/* webpackChunkName: "about" */ './views/About.vue')
    }
  ]
})
