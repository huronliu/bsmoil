module.exports = {
  outputDir: "../server/www",
  devServer: {
    port: 3000,
    proxy: {
      "/api": {
        target: "http://localhost:8080"
      },
      "/auth": {
        target: "http://localhost:8080"
      },
      "/asset": {
        target: "http://localhost:8080"
      }
    }
  },

  css: {
    sourceMap: true,
    modules: false
  },

  baseUrl: "/",
  assetsDir: "assets",
  runtimeCompiler: false,
  productionSourceMap: true,
  parallel: true,

  pwa: {
    name: "Base Station Monitor",
    workboxPluginMode: "GenerateSW"
  },
  lintOnSave: "error",
  configureWebpack: {
    devtool: "source-map"
  }
};
