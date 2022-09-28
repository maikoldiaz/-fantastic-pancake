const path = require('path');
const webpack = require('webpack');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const CaseSensitivePathsPlugin = require('case-sensitive-paths-webpack-plugin');
// const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;

const config = {
    entry: {
        // App
        main: [
            path.resolve(__dirname, './node_modules/@babel/polyfill/dist/polyfill.min.js'),
            path.resolve(__dirname, './wwwroot/src/js/index.js'),
            path.resolve(__dirname, './wwwroot/src/scss/ecp.vendor.scss'),
            path.resolve(__dirname, './wwwroot/src/scss/ecp.main.scss')
        ]
    },
    mode: 'development',
    output: {
        publicPath: '/dist/',
        path: path.resolve(__dirname, './wwwroot/dist/'),
        filename: 'js/ecp.[name].js'
    },
    devtool: false,
    plugins: [
        new webpack.DefinePlugin({
            'process.env': {
                NODE_ENV: JSON.stringify('development')
            }
        }),
        new webpack.SourceMapDevToolPlugin({
            filename: '[file].map',
            moduleFilenameTemplate: path.relative('./wwwroot/dist', '[resourcePath]')
        }),
        new MiniCssExtractPlugin({
            filename: 'css/ecp.[name].css'
        }),
        new webpack.HotModuleReplacementPlugin(),
        new CaseSensitivePathsPlugin()
        // new BundleAnalyzerPlugin()
    ],
    module: {
        rules: [
            {
                enforce: 'pre',
                test: /\.js$/,
                exclude: [/node_modules/],
                loader: 'eslint-loader',
                options: {
                    fix: true
                }
            },
            {
                test: /\.jsx?$/,
                loader: 'babel-loader',
                exclude: [/node_modules/],
                query: {
                    babelrc: false,
                    presets: [
                        ['@babel/preset-env', {
                            targets: {
                                node: true
                            }
                        }],
                        '@babel/preset-react',
                        {
                            'plugins': ['@babel/plugin-proposal-class-properties']
                        }
                    ]
                }
            },
            {
                test: /\.scss$/,
                use: [
                    MiniCssExtractPlugin.loader,
                    {
                        loader: 'css-loader',
                        options: {
                            url: false
                        }
                    },
                    'sass-loader'
                ]
            },
            {
                test: /\.(png|jpe?g|gif)$/i,
                loader: 'file-loader',
                options: {
                    name: '[path][name].[ext]'
                }
            }
        ]
    },
    node: {
        fs: 'empty'
    }
};

module.exports = config;

