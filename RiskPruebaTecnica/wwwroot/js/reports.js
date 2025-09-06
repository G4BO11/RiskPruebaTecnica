function generateReport() {
    const timeRange = document.getElementById('timeRange').value;
    const format = document.getElementById('format').value;
    
    window.location.href = `/Report/GenerateReport?format=${format}&timeRange=${timeRange}`;
}
