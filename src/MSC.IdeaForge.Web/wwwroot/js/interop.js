// MSC IdeaForge JavaScript interop yardımcıları
window.appInterop = {
    // Verilen CSV içeriğini dosya olarak indirir (UTF-8 BOM ile Türkçe karakter uyumlu)
    downloadCsv: function (filename, content) {
        const blob = new Blob(["﻿" + content], { type: 'text/csv;charset=utf-8;' });
        const url = URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = filename;
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        URL.revokeObjectURL(url);
    },

    // Verilen metni panoya kopyalar; başarı durumunu döner
    copyToClipboard: async function (text) {
        try {
            await navigator.clipboard.writeText(text);
            return true;
        } catch {
            return false;
        }
    },

};
