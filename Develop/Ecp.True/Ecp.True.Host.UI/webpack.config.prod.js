const path = require('path');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const TerserPlugin = require('terser-webpack-plugin');
const CompressionPlugin = require('compression-webpack-plugin');
const OptimizeCSSAssetsPlugin = require('optimize-css-assets-webpack-plugin');

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
    mode: 'production',
    output: {
        publicPath: '/dist/',
        path: path.resolve(__dirname, './wwwroot/dist/'),
        filename: 'js/ecp.[name].js'
    },
    plugins: [
        new CompressionPlugin({
            filename: '[path].gz[query]',
            algorithm: 'gzip',
            test: /\.js(\?.*)?$/i
        }),
        new MiniCssExtractPlugin({
            filename: 'css/ecp.[name].css'
        })
    ],
    module: {
        rules: [
            {
                test: /\.jsx?$/,
                loader: 'babel-loader',
                exclude: [/node_modules/],
                options: {
                    babelrc: false,
                    presets: [
                        ['@babel/preset-env', {
                            targets: {
                                node: true
                            }
                        }],
                        '@babel/preset-react'
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
    },
    optimization: {
        minimizer: [
            new TerserPlugin({
                parallel: true,
                terserOptions: {
                    ecma: 8
                }
            }),
            new OptimizeCSSAssetsPlugin({})
        ]
    }
};

module.exports = config;
