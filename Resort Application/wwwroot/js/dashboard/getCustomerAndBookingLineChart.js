﻿

$(document).ready(function () {
    loadCustomerBookingPieChart();

});


function loadCustomerBookingPieChart() {
    $(".chart-spiner").show();

    $.ajax({
        url: '/Dashboard/GetMemberAndBookingLineChartData',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            console.log("Pie Chart Data:", data);
            loadLineChart("newMembersAndBookingsLineChart", data);

            $(".chart-spiner").hide();
        }
    });
};


function loadLineChart(id,data) {
    var chartColors = getChartColorsArray(id);
    var options = {
        colors: chartColors,
        series: data.series,
        chart: {
            height: 350,
            type: 'line',
        },
        stroke: {
            show: false
        },
        markers: {
            size: 0,
            hover: {
                sizeOffset: 6
            }
        },
        xaxis: {
            categories: data.categories,
        },
    };
    var chart = new ApexCharts(document.querySelector("#" + id), options);
    chart.render();


};
