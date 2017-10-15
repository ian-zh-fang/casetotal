
/// <reference path="_references.js" />

(function ($, undefined) {

    var defaults = {
        bgcolor:'rgba(255, 255, 255, 0.7)',
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

    function _getTitle(total) {
        return [{
            text: '警情分布',
            textStyle: {
                color: '#000000',
                fontSize: 13
            },
            x: '18%',
            y: '10px'
        }, {
            text: '各辖区警情分布',
            textStyle: {
                color: '#000000',
                fontSize: 13
            },
            x: '55%',
            y: '10px'
        }, {
            text: '警情总数:\n\n' + total + ' 起',
            textStyle: { color: '#000000' },
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

    function _getLegend(data) {
        return {
            orient: 'horizontal',
            left: '35%',
            bottom: '20px',
            //itemWidth: 25,
            //itemHeight: 25,
            align: 'auto',
            //selectedMode: false,
            data: data.legendData,
            textStyle: { color: '#000000' }
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

    function _getStackBar(data, name, color) {
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
                    textStyle: { color: '#000000' }
                }
            },
            data: data
        }
    }

    function _getSeries(data, color) {
        var arr = [];
        var pieName = "警情分布";
        arr.push(_getInnerPie(data, pieName, color));
        arr.push(_getOuterPie(data, pieName, color));
        
        $.each(data.stackData, function (idx, o) {
            arr.push(_getStackBar(o.value, o.name, color));
        });

        return arr;
    }

    function _getOpts(options) {
        var color = options.colors;
        var data = _getData(options.data, color);
        var axis = _getAxis(data);

        return {
            backgroundColor: options.bgcolor,
            color: color,
            textStyle: { color: '#000000' },
            title: _getTitle(data.total),
            grid: _getGrid(),
            xAxis: axis.x,
            yAxis: axis.y,
            legend: _getLegend(data),
            series: _getSeries(data, color)
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

        var bgColor = opt.bgColor;
        var txtColor = opt.textColor;

        var data = [];
        var otherdata = [];
        var myData = ['杀人', '抢夺', '盗窃', '故意伤害', '诈骗', '抢劫', '侵犯人身', '侵犯财产', '纠纷', '交通事故'];
        for (var i = 0; i < myData.length; i++) {
            data.push(parseInt(Math.random() * (100 - 1 + 1) + 1));
            otherdata.push(parseInt(Math.random() * (100 - 1 + 1) + 1));
        }
        data = data.sort(function (a, b) { return b - a; });
        otherdata = otherdata.sort(function (a, b) { return a - b; });

        return {
            backgroundColor: bgColor,
            //color: ['#d74e67', '#eba954'],
            textStyle: {
                color: txtColor
            },
            legend: [{
                data: ['本周', '上周'],
                //selectedMode: false,
                bottom: '10px',
                left: '35%'
            }],
            grid: [{
                show: false,
                left: '38%',
                top: 60,
                bottom: 60,
                containLabel: false,
                width: '26%',
            }, {
                show: false,
                left: '68%',
                top: 80,
                bottom: 60,
                width: '0%',
            }, {
                show: false,
                right: '2%',
                top: 60,
                bottom: 60,
                containLabel: false,
                width: '26%',
            }, {
                show: false,
                left: '7%',
                top: '60px',
                width: '30.5%',
            }],
            xAxis: [
                {
                    type: 'value',
                    inverse: true,
                    axisLine: {
                        show: false,
                    },
                    axisTick: {
                        show: false,
                    },
                    position: 'top',
                    axisLabel: {
                        show: false,
                        textStyle: {
                            color: '#B2B2B2',
                            fontSize: 12,
                        },
                    },
                    splitLine: {
                        show: false,
                        lineStyle: {
                            color: '#B2B2B2',
                            width: 1,
                            type: 'dosh',
                        },
                    },
                }, {
                    gridIndex: 1,
                    show: true,
                }, {
                    gridIndex: 2,
                    type: 'value',
                    axisLine: {
                        show: false,
                    },
                    axisTick: {
                        show: false,
                    },
                    position: 'top',
                    axisLabel: {
                        show: false,
                        textStyle: {
                            color: '#B2B2B2',
                            fontSize: 12,
                        }
                    },
                    splitLine: {
                        show: false,
                        lineStyle: {
                            color: '#B2B2B2',
                            width: 1,
                            type: 'dosh',
                        },
                    }
                }, {
                    gridIndex: 3,
                    type: 'value',
                    position: 'top',
                    axisLine: {
                        show: false,
                    },
                    axisTick: {
                        show: false,
                    },
                    axisLabel: {
                        show: true,
                        textStyle: {
                            color: '#B2B2B2',
                            fontSize: 12,
                        }
                    },
                    splitLine: {
                        show: true,
                        lineStyle: {
                            color: '#B2B2B2',
                            width: 1,
                            type: 'dosh',
                        },
                    },
                    data: data
                }],
            yAxis: [
                {
                    type: 'category',
                    inverse: true,
                    position: 'right',
                    axisLine: {
                        show: false
                    },
                    axisTick: {
                        show: false
                    },
                    axisLabel: {
                        show: false,
                    },
                    data: myData
                }, {
                    gridIndex: 1,
                    type: 'category',
                    inverse: true,
                    position: 'left',
                    axisLine: {
                        show: false
                    },
                    axisTick: {
                        show: false
                    },
                    axisLabel: {
                        show: false,
                    },
                    data: myData.map(function (value) {
                        return {
                            value: value,
                            textStyle: {
                                align: 'center',
                            }
                        }
                    })
                }, {
                    gridIndex: 2,
                    type: 'category',
                    inverse: true,
                    position: 'left',
                    axisLine: {
                        show: false
                    },
                    axisTick: {
                        show: false
                    },
                    axisLabel: {
                        show: true,
                        textStyle: {
                            color: '#9D9EA0',
                            fontSize: 12,
                        }
                    },
                    data: myData,
                }, {
                    gridIndex: 3,
                    type: 'category',
                    inverse: true,
                    position: 'left',
                    axisLine: {
                        show: true
                    },
                    axisTick: {
                        show: false
                    },
                    axisLabel: {
                        show: true,
                    },
                    data: myData
                }],
            series: [{
                name: '本周',
                type: 'bar',
                barGap: 2,
                label: {
                    normal: {
                        show: false,
                    },
                    emphasis: {
                        show: true,
                        position: 'left',
                        offset: [0, 0],
                        textStyle: {
                            color: '#fff',
                            fontSize: 14
                        }
                    }
                },
                itemStyle: {
                    normal: {
                        color: '#659F83'
                    },
                    emphasis: {
                        color: '#08C7AE'
                    },
                },
                data: data,
            }, {
                name: '上周',
                type: 'bar',
                barGap: 2,
                xAxisIndex: 2,
                yAxisIndex: 2,
                label: {
                    normal: {
                        show: false
                    },
                    emphasis: {
                        show: true,
                        position: 'right',
                        offset: [0, 0],
                        textStyle: {
                            color: '#fff',
                            fontSize: 14
                        }
                    }
                },
                itemStyle: {
                    normal: {
                        color: '#F68989'
                    },
                    emphasis: {
                        color: '#F94646'
                    }
                },
                data: data
            }, {
                xAxisIndex: 3,
                yAxisIndex: 3,
                name: '本周',
                type: 'line',
                data: data,
            }, {
                xAxisIndex: 3,
                yAxisIndex: 3,
                name: '上周',
                type: 'line',
                data: otherdata,
            }]
        }
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