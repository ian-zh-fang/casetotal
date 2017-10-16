
/// <reference path="_references.js" />

(function ($, undefined) {

    var defaults = {
        bgColor: '#404A59',
        textColor:'#ffffff',
        colors: ['#d74e67', '#eba954', '#5b8144'],
        data: []
    };

    // init example data start

    var x = 10;
    var y = 0;
    for (var i = 1; i < 23; i++) {

        var data = [
            { name: '刑事', value: i },
            { name: '治安', value: i },
            { name: '其它', value: parseInt(Math.random() * (x - y + 1) + y) }
        ];

        defaults.data.push({
            name: '派出所' + i,
            value: data,
            total: data[0].value + data[1].value + data[2].value
        });
    }
    defaults.data.sort(function (a, b) { return b.total - a.total; });

    // init example data end

    function _draw(jq, opt) {

        var doc = document.getElementById($(jq).selector);
        var cht = echarts.init(doc);
        cht.setOption(opt);
    }

    function _getData(data, color) {
        var total = 0;
        var yAxisData = [];
        var innerPieData = [];
        var outerPieData = [];
        var stackData = [];
        var legendData = [];

        $.each(data, function (i, o) {

            var d = { name: o.name, value: [] };
            $.each(o.value, function (j, e) {
                total += e.value;
                d.value.push(e.value);

                var idx = $.inArray(e.name, legendData);
                if (-1 == idx) {
                    outerPieData.push({ name: e.name, value: e.value });
                    innerPieData.push({ name: '', value: e.value });

                    var s = { name: e.name, value: [] };
                    s.value.push(e.value);
                    stackData.push(s);

                    legendData.push(e.name);
                } else {
                    var val = outerPieData[idx].value + e.value;

                    outerPieData[idx].value = val;
                    innerPieData[idx].value = val;
                    stackData[idx].value.push(e.value);
                }
            });

            yAxisData.push(o.name);
        });

        return {
            yAxisData: yAxisData,
            innerPieData: innerPieData,
            outerPieData: outerPieData,
            stackData: stackData,
            legendData: legendData,
            total: total
        };
    }

    function _getTitle(total, txtColor) {
        return [{
            text: '警情分布',
            textStyle: {
                color: txtColor,
                fontSize: 13
            },
            x: '18%',
            y: '10px'
        }, {
            text: '各辖区警情分布',
            textStyle: {
                color: txtColor,
                fontSize: 13
            },
            x: '55%',
            y: '10px'
        }, {
            text: '警情总数:\n\n' + total + ' 起',
            textStyle: { color: '#ffd285' },
            x: '18%',
            y: '43%'
        }];
    }

    function _getGrid() {
        return [{ top: '10%', left: '50%', 　width: '45%' }];
    }

    function _getYAxis(data, gridIndex) {
        return [{
            gridIndex: gridIndex,
            type: 'category',
            data: data.yAxisData,
            axisTick: { inside: false, show: false },
            axisLabel: { show: true, interval: 0 },
            inverse: true
        }];
    }
    
    function _getAxis(data) {
        return {
            x: [{
                gridIndex: 0,
                type: "value",
                axisLabel: { "show": false },
                axisLine: { "show": false },
                axisTick: { "show": false },
                splitLine: { "show": false }
            }],
            y: _getYAxis(data, 0)
        };
    }

    function _getLegend(data, txtColor) {
        return {
            orient: 'horizontal',
            left: '35%',
            bottom: '20px',
            //itemWidth: 25,
            //itemHeight: 25,
            align: 'auto',
            //selectedMode: false,
            data: data.legendData,
            textStyle: { color: txtColor }
        };
    }

    function _getInnerPie(data, name, color) {
        return {
            name: name,
            type: 'pie',
            hoverAnimation: false,
            legendHoverLink: false,
            radius: ['40%', '42%'],
            center: ['20%', '50%'],
            //color: color,
            label: {
                normal: { position: 'inner', show: false }
            },
            labelLine: {
                normal: { show: false }
            },
            tooltip: { show: false },
            data: data.outerPieData
        };
    }

    function _getOuterPie(data, name, color) {
        return {
            name: name,
            type: 'pie',
            center: ['20%', '50%'],
            radius: ['42%', '55%'],
            //color: color,
            label: {
                normal: { formatter: '{b}\n{c}({d}%)' }
            },
            data: data.outerPieData
        };
    }

    function _getStackBar(data, name, color, txtColor) {
        return {
            name: name,
            type: "bar",
            xAxisIndex: 0,
            yAxisIndex: 0,
            stack: "辖区警情分布",
            //color: color,
            label: {
                normal: {
                    show: false,
                    textStyle: { color: txtColor }
                }
            },
            data: data
        }
    }

    function _getSeries(data, color, txtColor) {
        var arr = [];
        var pieName = "警情分布";
        arr.push(_getInnerPie(data, pieName, color));
        arr.push(_getOuterPie(data, pieName, color));
        
        $.each(data.stackData, function (idx, o) {
            arr.push(_getStackBar(o.value, o.name, color, txtColor));
        });

        return arr;
    }

    function _getOpts(options) {
        var color = options.colors;
        var txtcolor = options.textColor;
        var data = _getData(options.data, color);
        var axis = _getAxis(data);

        return {
            backgroundColor: options.bgColor,
            color: color,
            textStyle: { color: txtcolor },
            title: _getTitle(data.total, txtcolor),
            grid: _getGrid(),
            xAxis: axis.x,
            yAxis: axis.y,
            legend: _getLegend(data, txtcolor),
            series: _getSeries(data, color, txtcolor)
        };
    }

    $.fn.stackpie = function (options) {
        debugger;
        var opts = $.extend({}, this.stackpie.defaults, options);
        opts = _getOpts(opts);

        var doc = this;
        return {
            draw: function () { _draw(doc, opts); }
        };
    };

    $.fn.stackpie.defaults = defaults;

})(jQuery, undefined);

(function ($, undefined) {

    function _getTitle(color) {
        return [{
            text: '环比',
            textStyle: {
                color: color
            },
            x: '19%',
            y: '10px'
        }];
    }

    function _getGrid() {
        return [
            { top: '10%', left: '5%', width: '45%'},
            { top: '10%', left: '55%', width: '45%' }
        ];
    }

    function _getAxi(data) {
        return {
            x: {

            },
            y: {

            }
        };
    }

    function _getAxis(data) {
        return {
            x: [],
            y: []
        };
    }

    function _getOpt(opt) {

        var option = {
            backgroundColor: "#404A59",
            color: ['#ffd285', '#ff733f', '#ec4863'],

            title: [{
                text: '警情环比分析',
                left: '50%',
                top: '20px',
                textStyle: {
                    color: '#fff'
                }
            }],
            tooltip: {
                trigger: 'axis'
            },
            legend: {
                x: 300,
                top: '7%',
                textStyle: {
                    color: '#ffd285',
                },
                data: ['上周', '本周']
            },
            grid: [{
                left: '1%',
                top: '20%',
                bottom: '6%',
                width: '45%',
                containLabel: true
            }, {
                right: '5%',
                top: '20%',
                bottom: '6%',
                width: '30%',
                containLabel: true,
                show: false
            }],
            toolbox: {
                "show": false,
                feature: {
                    saveAsImage: {}
                }
            },
            xAxis: [{
                type: 'category',
                "axisLine": {
                    lineStyle: {
                        color: '#FF4500'
                    }
                },
                "axisTick": {
                    "show": false
                },
                axisLabel: {
                    textStyle: {
                        color: '#fff'
                    }
                },
                boundaryGap: false,
                data: ['周一', '周二', '周三', '周四', '周五', '周六', '周日']
            }, {
                gridIndex: 1,
                type: 'category',
                "axisLine": {
                    lineStyle: {
                        color: '#FF4500'
                    }
                },
                "axisTick": {
                    "show": false
                },
                axisLabel: {
                    textStyle: {
                        color: '#fff'
                    }
                },
                boundaryGap: false,
                data: ['周一', '周二', '周三', '周四', '周五', '周六', '周日'],
                axisTick: {
                    alignWithLabel: true
                }
            }],
            yAxis: [{
                name: '起数',
                axisLine: {
                    show: false,
                    lineStyle: {
                        color: '#fff'
                    }
                },
                splitLine: {
                    show: true,
                    lineStyle: {
                        color: '#fff'
                    }
                },
                "axisTick": {
                    "show": false
                },
                axisLabel: {
                    textStyle: {
                        color: '#fff'
                    }
                },
                type: 'value'
            }, {
                gridIndex: 1,
                axisLine: {
                    show: false,
                    lineStyle: {
                        color: '#fff'
                    }
                },
                splitLine: {
                    show: false,
                    lineStyle: {
                        color: '#fff'
                    }
                },
                axisLabel: {
                    show: false,
                    textStyle: {
                        color: '#fff'
                    }
                },
                axisTick: {
                    alignWithLabel: true,
                    show: false
                }
            }],
            series: [{
                name: '上周',
                smooth: true,
                type: 'line',
                symbolSize: 8,
                symbol: 'circle',
                data: [90, 50, 39, 50, 120, 82, 80]
            }, {
                name: '本周',
                smooth: true,
                type: 'line',
                symbolSize: 8,
                symbol: 'circle',
                data: [70, 50, 50, 87, 90, 80, 70]
            },
            {
                type: 'pie',
                center: ['57%', '50%'],
                radius: ['45%', '50%'],
                label: {
                    normal: {
                        position: 'center',
                        formatter: '{b}\n{c}({d}%)'
                    }
                },
                data: [{
                    value: 335,
                    name: '环比下降',
                    itemStyle: {
                        normal: {
                            color: '#ffd285'
                        }
                    },
                    label: {
                        normal: {
                            formatter: '{d} %',
                            textStyle: {
                                color: '#ffd285',
                                fontSize: 20

                            }
                        }
                    }
                }, {
                    value: 180,
                    name: '占位',
                    tooltip: {
                        show: true
                    },
                    itemStyle: {
                        normal: {
                            color: '#87CEFA'
                        }
                    },
                    label: {
                        normal: {
                            textStyle: {
                                color: '#ffd285',
                            },
                            formatter: '\n环比下降'
                        }
                    }
                }]
            }, {
                name: '本周',
                xAxisIndex: 1,
                yAxisIndex: 1,
                type: 'bar',
                label: {
                    normal: {
                        position: 'top',
                        show: true,
                        formatter: "{c}"
                    }
                },
                data: [1, 3, 2, 33, 4, 12, 1]
            }, {
                name: '上周',
                xAxisIndex: 1,
                yAxisIndex: 1,
                type: 'bar',
                label: {
                    normal: {
                        position: 'top',
                        show: true,
                        formatter: "{c}"
                    }
                },
                data: [5, 31, 2, 3, 4, 2, 11]
            }],
            //label: {
            //    normal: {
            //        show: true,
            //        position: 'top',
            //        formatter: '{c}'
            //    }
            //},
            //itemStyle: {
            //    normal: {

            //        color: new echarts.graphic.LinearGradient(0, 0, 0, 1, [{
            //            offset: 0,
            //            color: 'rgba(17, 168,171, 1)'
            //        }, {
            //            offset: 1,
            //            color: 'rgba(17, 168,171, 0.1)'
            //        }]),
            //        shadowColor: 'rgba(0, 0, 0, 0.1)',
            //        shadowBlur: 10
            //    }
            //}
        };
        return option;
    }

    function _draw(jq, opt) {
        var doc = document.getElementById($(jq).selector);
        var cht = echarts.init(doc);
        cht.setOption(opt);
    }

    $.fn.stackline = function (opt) {
        debugger;
        var opts = $.extend({}, this.stackline.defaults, opt);
        opts = _getOpt(opts);

        var jq = this;
        return {
            draw: function () { _draw(jq, opts); }
        }
    };

    var defaults = {
        bgColor: 'rgba(255, 255, 255, 0.7)',
        textColor: '#000000',
        data:[]
    };

    // init example data start

    // init example data end
    defaults.data.sort(function (a, b) {

        return true;
    });
    $.fn.stackline.defaults = defaults;

})(jQuery, undefined);