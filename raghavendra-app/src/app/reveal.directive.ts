import { Directive, ElementRef, HostListener, Renderer2 } from '@angular/core';

@Directive({
  selector: '[appReveal]',
})
export class RevealDirective {
  revealpoint = 150;

  constructor(private elementRef: ElementRef, private renderer: Renderer2) {}
  //// This is used for revealing the element on scroll since we were not able to use the intersection API
  /// Another way of solution acheived is using scrolling
  @HostListener('document:scroll') scrollonReveal(eventData: Event) {
    let windowheight = window.innerHeight;
    let revealtop = this.elementRef.nativeElement.getBoundingClientRect().top;
    if (revealtop < windowheight - this.revealpoint) {
      this.renderer.addClass(this.elementRef.nativeElement, 'animation');
    } else {
      this.renderer.removeClass(this.elementRef.nativeElement, 'animation');
      
    }
  }
}
