import decode from 'jwt-decode';

export default {
  getValue(key) {
    return window.localStorage.getItem(key);
  },
  setValue(key, value) {
    return window.localStorage.setItem(key, value);
  },
  getSessionValue(key) {
    return window.sessionStorage.getItem(key);
  },
  setSessionValue(key, value) {
    return window.sessionStorage.setItem(key, value);
  },

  getAppStarted() {
    if (this.getValue('app.started') && this.getValue('app.started') === 'true') 
      return true;
    else 
      return false;
  },
  setAppStarted() {
    this.setValue('app.started', 'true');
  },
  getServiceUrl() {
    let url = this.getValue('bsm_service_url');
    if (!url) {
      url = 'http://localhost:8089';
    }
    return url;
  },
  setServiceUrl(url) {
    this.setValue('bsm_service_url', url);
  },
  getLoginName() {
    return this.getValue('bsm_loginname');
  },
  setLoginName(name) {
    this.setValue('bsm_loginname', name);
  },
  getPassword() {
    return this.getValue('bsm_loginpass');
  },
  setPassword(pass) {
    this.setValue('bsm_loginpass', pass);
  },
  getRememberMe() {
    let remember = this.getValue('bsm_remember');
    return remember === 'true'? true : false;
  },
  setRememberMe(remember) {
    this.setValue('bsm_remember', remember? 'true' : 'false');
  },

  getToken() {
    return this.getSessionValue('bsm_token');
  },
  setToken(token) {
    this.setSessionValue('bsm_token', token);
  },
  getUser() {
    let user = this.getSessionValue('bsm_user');
    if (user) {
      return JSON.parse(user);
    }
    return null;
  },
  setUser(user) {
    if (user) {
      this.setSessionValue('bsm_user', JSON.stringify(user));  
    }    
  },
  isTokenValid() {
    let token = this.getToken();
    if (token) {
      const jwt = decode(token);
      var dateNow = new Date();
      if (jwt.exp && jwt.exp > dateNow.getTime() / 1000) {
        return true;
      }
    }    
    return false;
  },
  cleanUserSession() {
    window.sessionStorage.clear();
  }
}