import Vue from "vue";
import Router from "vue-router";
import Stations from "./views/Stations";
import StationDetail from "./views/StationDetail";
import Warns from "./views/Warns";
import WarnDetail from "./views/WarnDetail";
import WarnsHistory from "./views/WarnsHistory";
import Me from "./views/Me";
import Home from "./views/Home";
import Startup from "./views/Startup";
import Login from "./views/Login";
import Config from "./views/Config";
import Setting from "./views/Setting";

Vue.use(Router);

export default new Router({
  mode: "history",
  routes: [
    {
      path: "/login",
      name: "login",
      component: Login,
      meta: {
        requireAuth: false,
        footer: false
      }
    },
    {
      path: "/start",
      name: "start",
      component: Startup,
      meta: {
        requireAuth: false,
        footer: false 
      }
    },
    {
      path: "/",
      name: "home",
      component: Home,
      meta: {
        requireAuth: false,
        roles: []
      }
    },
    {
      path: "/stations",
      name: "stations",
      component: Stations,
      meta: {
        requireAuth: true,
        roles: []
      },
      props: true
    },
    {
      path: "/stationdetail",
      name: "stationDetail",
      component: StationDetail,
      meta: {
        requireAuth: true,
        roles: []
      },
      props: true
    },
    {
      path: "/warns",
      name: "warns",
      component: Warns,
      meta: {
        requireAuth: true,
        roles: []
      }
    },
    {
      path: "/warndetail",
      name: "warnDetail",
      component: WarnDetail,
      meta: {
        requireAuth: true, 
        roles: []
      },
      props: true
    },
    {
      path: "/warnshistory",
      name: "warnsHistory",
      component: WarnsHistory,
      meta: {
        requireAuth: true, 
        roles: []
      },
      props: true
    },
    {
      path: "/setting",
      name: "setting",
      component: Setting,
      meta: {
        requireAuth: false,
        footer: false
      }
    },
    {
      path: "/config",
      name: "/config",
      component: Config,
      meta: {
        requireAuth: true,
        roles: []
      }
    },
    {
      path: "/me",
      name: "me",
      component: Me,
      meta: {
        requireAuth: true,
        roles: []
      }
    },
    { path: '*', redirect: '/' }
  ]
});
