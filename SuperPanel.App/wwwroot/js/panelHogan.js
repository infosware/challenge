
class PanelHogan {
    constructor(templateId) {
        this.templateId = templateId;
        this.template = null;
        this.compiledTemplate = null;
        this.renderedTemplate = null;

        this.getTemplate();
        this.compileTemplate();
    }

    getTemplate() {
        this.template = window[this.templateId].innerHTML;
    }

    compileTemplate() {
        this.compiledTemplate = Hogan.compile(this.template);
    }

    renderTemplate(object) {
        this.renderedTemplate = this.compiledTemplate.render({ object: object });
        return this.renderedTemplate;
    }
}

export { PanelHogan };
