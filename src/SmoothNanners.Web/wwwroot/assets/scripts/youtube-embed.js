"use strict";

const containers = document.querySelectorAll(".youtube-embed-container");

containers.forEach(container => container.querySelector("a").addEventListener("click", e => {
    e.preventDefault();

    // Pause any other embeds that may be playing. Requires "enablejsapi=1" query parameter in the iframe src. 
    containers.forEach(c => c.querySelectorAll("iframe").forEach(iframe => iframe.contentWindow.postMessage(JSON.stringify({
        event: "command",
        func: "pauseVideo"
    }), "*")));

    // Replace contents of container with embed.
    const embed = document.getElementById("youtube-embed-template").content.cloneNode(true);
    embed.querySelector("iframe").src = `https://www.youtube.com/embed/${container.dataset.videoId}?autoplay=1&enablejsapi=1`;
    container.replaceChildren(embed);
}));
