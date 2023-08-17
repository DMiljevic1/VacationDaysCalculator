function download(resultByte, fileName) {
    var bytes = new Uint8Array(resultByte); // pass your byte response to this constructor

    var blob = new Blob([bytes], { type: "application/pdf" });// change resultByte to bytes

    var link = document.createElement('a');
    link.href = window.URL.createObjectURL(blob);
    link.download = fileName;
    link.click();
}