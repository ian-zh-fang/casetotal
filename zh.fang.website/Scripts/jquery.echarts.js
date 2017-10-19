
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
    var size = 16;
    for (var i = 1; i < size; i++) {

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
            x: '13%',
            y: '10px'
        }, {
            text: '各辖区警情分布',
            textStyle: {
                color: txtColor,
                fontSize: 13
            },
            x: '50%',
            y: '10px'
        }, {
            text: '警情总数:\n\n' + total + ' 起',
            textStyle: { color: '#ffd285' },
            x: '13%',
            y: '43%'
        }, {
            text: '警情分类详情',
            textStyle: {
                color: txtColor,
                fontSize: 13
            },
            right: '10%',
            top: '10px'
        }];
    }

    function _getGrid() {
        return [{ top: '20%', left: '35%', width: '39%', show: false }, {
            top: '10%', right: '3%', width: '20%', show: false
        }];
    }

    function _getYAxis(data, gridIndex) {
        return [{
            gridIndex: gridIndex,
            type: 'category',
            data: data.yAxisData,
            axisTick: { inside: false, show: false },
            axisLine: { show: false },
            axisLabel: { show: true, interval: 0 },
            inverse: true
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
            }, {
                    gridIndex: 1,
                    type: 'category',
                    "axisLine": {
                        "show": false
                    },
                    "axisTick": {
                        "show": true
                    },
                    axisLabel: {
                        "show": true
                    },
                    boundaryGap: false,
                    data: ["杀人", "抢夺", "抢劫", "盗窃", "交通事故", "求助", "纠纷"],
                    axisTick: {
                        "show": false,
                        alignWithLabel: false
                    }
            }],
            y: _getYAxis(data, 0)
        };
    }

    function _getLegend(data, txtColor) {
        return [{
            orient: 'horizontal',
            left: '25%',
            bottom: '20px',
            //itemWidth: 25,
            //itemHeight: 25,
            align: 'auto',
            //selectedMode: false,
            data: data.legendData,
            textStyle: { color: txtColor }
        }];
    }

    function _getInnerPie(data, name, color) {
        return {
            name: name,
            type: 'pie',
            hoverAnimation: false,
            legendHoverLink: false,
            center: ['15%', '50%'],
            radius: ['49%', '50%'],
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
            center: ['15%', '50%'],
            radius: ['50%', '55%'],
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
            //barGap: '30%',
            barMaxWidth:'7px',
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

        arr.push({
            name: '警情类型',
            xAxisIndex: 1,
            yAxisIndex: 1,
            type: 'bar',
            //barWidth: 15,
            itemStyle: {
                normal: {
                    barBorderRadius: 0
                }
            },
            label: {
                normal: {
                    position: 'top',
                    show: true,
                    formatter: "{c}"
                }
            },
            data: [5, 31, 2, 3, 4, 2, 11]
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
                text: '近两周警情环比分析',
                left: '43%',
                top: '20px',
                textStyle: {
                    color: '#fff',
                    align:'center'
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
                    "show": true
                },
                axisLabel: {
                    textStyle: {
                        color: '#fff'
                    }
                },
                boundaryGap: false,
                data: ['周一', '周二', '周三', '周四', '周五', '周六', '周日']
            },
            {
                gridIndex: 1,
                type: 'category',
                "axisLine": {
                    "show": false,
                    lineStyle: {
                        color: '#FF4500'
                    }
                },
                "axisTick": {
                    "show": false
                },
                axisLabel: {
                    "show": false,
                    textStyle: {
                        color: '#fff'
                    }
                },
                boundaryGap: false,
                data: ['周一', '周二', '周三', '周四', '周五', '周六', '周日'],
                axisTick: {
                    "show": false,
                    alignWithLabel: false
                }
            }
            ],
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
                data: function () {
                    var arr = [];
                    var size = 7;

                    var x = 150;
                    var y = 2;
                    for (var i = 0; i < size; i++) {
                        var e = parseInt(Math.random() * (x - y + 1) + y);
                        arr.push(e);
                    }
                    return arr.sort(function (a, b) { return b - a; });
                }()
            }, {
                name: '本周',
                smooth: true,
                type: 'line',
                symbolSize: 8,
                symbol: 'circle',
                data: function () {
                    var arr = [];
                    var size = 7;
                    var x = 205;
                    var y = 10;
                    for (var i = 0; i < size; i++) {
                        var e = parseInt(Math.random() * (x - y + 1) + y);
                        arr.push(e);
                    }
                    return arr.sort(function (a, b) { return b - a; });
                }()
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
            },
            {
                type: 'pie',
                center: ['73%', '50%'],
                radius: ['45%', '50%'],
                label: {
                    normal: {
                        position: 'center',
                        formatter: '{b}\n{c}({d}%)'
                    }
                },
                data: [{
                    value: 218,
                    name: '环比下降',
                    itemStyle: {
                        normal: {
                            color: '#ffd285'
                        }
                    },
                    label: {
                        normal: {
                            formatter: '两抢一盗\n\n{d} %',
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
            },
            {
                type: 'pie',
                center: ['89%', '50%'],
                radius: ['45%', '50%'],
                label: {
                    normal: {
                        position: 'center',
                        formatter: '{b}\n{c}({d}%)'
                    }
                },
                data: [{
                    value: 35,
                    name: '环比下降',
                    itemStyle: {
                        normal: {
                            color: '#ffd285'
                        }
                    },
                    label: {
                        normal: {
                            formatter: '其它警情\n\n{d} %',
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
                            formatter: '\n环比下上升'
                        }
                    }
                }]
            },
            //{
            //    name: '本周',
            //    xAxisIndex: 1,
            //    yAxisIndex: 1,
            //    type: 'bar',
            //    label: {
            //        normal: {
            //            position: 'top',
            //            show: true,
            //            formatter: "{c}"
            //        }
            //    },
            //    data: [1, 3, 2, 33, 4, 12, 1]
            //}, {
            //    name: '上周',
            //    xAxisIndex: 1,
            //    yAxisIndex: 1,
            //    type: 'bar',
            //    label: {
            //        normal: {
            //            position: 'top',
            //            show: true,
            //            formatter: "{c}"
            //        }
            //    },
            //    data: [5, 31, 2, 3, 4, 2, 11]
            //}
            ]
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
            { top: '10%', left: '5%', width: '45%' },
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
                text: '8，9 月警情环比分析',
                left: '43%',
                top: '20px',
                textStyle: {
                    color: '#fff',
                    align: 'center'
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
                data: ['8 月', '9 月']
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
                    "show": true
                },
                axisLabel: {
                    textStyle: {
                        color: '#fff'
                    }
                },
                boundaryGap: false,
                data: function () {
                    var arr = [];
                    var size = 32;
                    for (var i = 1; i < size; i++) {
                        arr.push(i + '日');
                    }
                    return arr;
                }()
            },
            {
                gridIndex: 1,
                type: 'category',
                "axisLine": {
                    "show": false,
                    lineStyle: {
                        color: '#FF4500'
                    }
                },
                "axisTick": {
                    "show": false
                },
                axisLabel: {
                    "show": false,
                    textStyle: {
                        color: '#fff'
                    }
                },
                boundaryGap: false,
                data: ['周一', '周二', '周三', '周四', '周五', '周六', '周日'],
                axisTick: {
                    "show": false,
                    alignWithLabel: false
                }
            }
            ],
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
                name: '8 月',
                smooth: true,
                type: 'line',
                symbolSize: 8,
                symbol: 'circle',
                data: function () {
                    var arr = [];
                    var size = 31;

                    var x = 150;
                    var y = 2;
                    for (var i = 0; i < size; i++) {
                        var e = parseInt(Math.random() * (x - y + 1) + y);
                        arr.push(e);
                    }
                    return arr.sort(function (a, b) { return b - a; });
                }()
            }, {
                name: '9 月',
                smooth: true,
                type: 'line',
                symbolSize: 8,
                symbol: 'circle',
                data: function () {
                    var arr = [];
                    var size = 31;
                    var x = 205;
                    var y = 10;
                    for (var i = 0; i < size; i++) {
                        var e = parseInt(Math.random() * (x - y + 1) + y);
                        arr.push(e);
                    }
                    return arr.sort(function (a, b) { return b - a; });
                }()
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
            },
            {
                type: 'pie',
                center: ['73%', '50%'],
                radius: ['45%', '50%'],
                label: {
                    normal: {
                        position: 'center',
                        formatter: '{b}\n{c}({d}%)'
                    }
                },
                data: [{
                    value: 218,
                    name: '环比下降',
                    itemStyle: {
                        normal: {
                            color: '#ffd285'
                        }
                    },
                    label: {
                        normal: {
                            formatter: '两抢一盗\n\n{d} %',
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
            },
            {
                type: 'pie',
                center: ['89%', '50%'],
                radius: ['45%', '50%'],
                label: {
                    normal: {
                        position: 'center',
                        formatter: '{b}\n{c}({d}%)'
                    }
                },
                data: [{
                    value: 35,
                    name: '环比下降',
                    itemStyle: {
                        normal: {
                            color: '#ffd285'
                        }
                    },
                    label: {
                        normal: {
                            formatter: '其它警情\n\n{d} %',
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
                            formatter: '\n环比下上升'
                        }
                    }
                }]
            },
                //{
                //    name: '本周',
                //    xAxisIndex: 1,
                //    yAxisIndex: 1,
                //    type: 'bar',
                //    label: {
                //        normal: {
                //            position: 'top',
                //            show: true,
                //            formatter: "{c}"
                //        }
                //    },
                //    data: [1, 3, 2, 33, 4, 12, 1]
                //}, {
                //    name: '上周',
                //    xAxisIndex: 1,
                //    yAxisIndex: 1,
                //    type: 'bar',
                //    label: {
                //        normal: {
                //            position: 'top',
                //            show: true,
                //            formatter: "{c}"
                //        }
                //    },
                //    data: [5, 31, 2, 3, 4, 2, 11]
                //}
            ]
        };
        return option;
    }

    function _draw(jq, opt) {
        var doc = document.getElementById($(jq).selector);
        var cht = echarts.init(doc);
        cht.setOption(opt);
    }

    $.fn.stackline2 = function (opt) {
        debugger;
        var opts = $.extend({}, this.stackline2.defaults, opt);
        opts = _getOpt(opts);

        var jq = this;
        return {
            draw: function () { _draw(jq, opts); }
        }
    };

    var defaults = {
        bgColor: 'rgba(255, 255, 255, 0.7)',
        textColor: '#000000',
        data: []
    };

    // init example data start

    // init example data end
    defaults.data.sort(function (a, b) {

        return true;
    });
    $.fn.stackline2.defaults = defaults;

})(jQuery, undefined);