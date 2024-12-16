import { Component, Input } from '@angular/core';
import { Message } from '../../models/messages';

@Component({
  selector: 'app-human-message',
  imports: [],
  templateUrl: './human-message.component.html',
  styleUrl: './human-message.component.scss',
  standalone: true
})
export class HumanMessageComponent {
  @Input() message?: Message;
}
