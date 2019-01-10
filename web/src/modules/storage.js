export default {
  getValue(key) {
    return window.localStorage.getItem(key);
  },
  setValue(key, value) {
    return window.localStorage.setItem(key, value);
  },

  getAppStarted() {
    if (this.getValue('app.started') && this.getValue('app.started') === 'true') 
      return true;
    else 
      return false;
  },
  setAppStarted() {
    this.setValue('app.started', 'true');
  }
}