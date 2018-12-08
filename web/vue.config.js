module.exports = {
  outputDir: "../mobile/www",
  devServer: {
    port: 3000
  },
  baseUrl: "./",
  assetsDir: "assets",
  lintOnSave: process.env.NODE_ENV !== 'production',
  productionSourceMap: true
};
