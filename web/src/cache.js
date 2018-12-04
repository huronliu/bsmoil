import _ from "lodash";

function Cache(w) {
  this.storage = w.localStorage;
  this.dataProviderMap = {};
}
function getKey(key) {
  return `cache_${key}`;
}
Cache.prototype.getObj = function(key, defaultValue) {
  const str = this.storage.getItem(getKey(key));
  return str ? JSON.parse(str) : defaultValue;
};
Cache.prototype.getString = function(key, defaultValue) {
  return this.storage.getItem(getKey(key)) || defaultValue;
};
Cache.prototype.getBoolean = function(key, defaultValue) {
  const str = this.storage.getItem(getKey(key));
  return str ? str === "true" : defaultValue;
};
Cache.prototype.setObj = function(key, value) {
  this.storage.setItem(getKey(key), JSON.stringify(value));
};
Cache.prototype.setString = function(key, value) {
  this.storage.setItem(getKey(key), value);
};
Cache.prototype.setBoolean = function(key, value) {
  this.storage.setItem(getKey(key), !!value);
};
Cache.prototype.remove = function(key) {
  this.storage.removeItem(getKey(key));
};
Cache.prototype.setDataProvider = function(key, dataProvider) {
  this.dataProviderMap[key] = dataProvider;
};
Cache.prototype.clearAll = function() {
  const keys = _.keys(this.storage);
  for (const key of keys) {
    this.storage.removeItem(getKey(key));
  }
};
Cache.prototype.clear = function(key) {
  this.storage.removeItem(getKey(key));
};
Cache.prototype.isInitialized = function(key) {
  return !!this.storage.getItem(getKey(key));
};
Cache.prototype.initialize = function(keys) {
  const newKeys = _.isEmpty(keys) ? _.keys(this.dataProviderMap) : keys;
  const providers = [];
  const uncachedKeys = [];
  for (const key of newKeys) {
    if (!this.isInitialized(key)) {
      providers.push(this.dataProviderMap[key]());
      uncachedKeys.push(key);
    }
  }

  if (providers.length > 0) {
    return Promise.all(providers).then(result => {
      _.forEach(result, (value, index) => {
        const key = uncachedKeys[index];
        this.setObj(key, value);
      });
    });
  } else {
    return Promise.resolve({});
  }
};

export default new Cache(window);
