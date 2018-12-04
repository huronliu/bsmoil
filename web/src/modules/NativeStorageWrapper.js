function NativeStorageWrapper() {}
NativeStorageWrapper.prototype.getString = function(key) {
  return new Promise((resolve, reject) => {
    window.NativeStorage.getString(
      key,
      value => resolve(value),
      err => reject(err)
    );
  });
};
NativeStorageWrapper.prototype.setString = function(key, value) {
  return new Promise((resolve, reject) => {
    window.NativeStorage.putString(
      key,
      value,
      value => resolve(value),
      err => reject(err)
    );
  });
};

export default new NativeStorageWrapper();
