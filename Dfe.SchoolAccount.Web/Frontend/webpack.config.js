const path = require("path");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");

module.exports = {
  mode: "production",
  entry: {
    main: [
      path.resolve(__dirname, "./js/main.js"),
      path.resolve(__dirname, "./scss/main.scss"),
    ],
  },
  plugins: [
    new MiniCssExtractPlugin({
      filename: "css/main.bundle.css",
    }),
  ],
  module: {
    rules: [
      {
        test: /\.s[ac]ss$/i,
        use: [
          MiniCssExtractPlugin.loader,
          "css-loader",
          "sass-loader",
        ],
      },
    ]
  },
  output: {
    path: path.resolve(__dirname, "../wwwroot"),
    filename: "js/[name].bundle.js",
  }
};
