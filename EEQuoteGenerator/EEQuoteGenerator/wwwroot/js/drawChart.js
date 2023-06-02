
$(document).ready(function () {
    google.charts.load('current', {
        'packages': ['bar']
    });
    google.charts.setOnLoadCallback(function () {
        drawChart();
    });

    function drawChart() {
        var data = new google.visualization.DataTable();
        data.addColumn('string', 'Region');
        data.addColumn('number', 'Quotes');
        var dataArray = [];

        Object.entries(RegionWiseQuotesData).forEach(([key, value]) => {
            dataArray.push([key, value]);
        });

        data.addRows(dataArray);

        var options = {
            chart: {
                title: 'Graph'
            },
            colors: ['#96BB22'],
            height: 250,
            width: 450
        };

        var barchart = new google.charts.Bar(document
            .getElementById('barchart_div'));
        barchart.draw(data, options);
    }

})
