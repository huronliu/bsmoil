export function TMap(key) {
  return new Promise(function(resolve, reject) {
    window.init = function() {
      resolve(window.qq);
    };
    let script = document.createElement("script");
    script.type = "text/javascript";
    script.src = "https://map.qq.com/api/js?v=2.exp&callback=init&key=" + key;
    script.onerror = reject;
    document.head.appendChild(script);
  });
}