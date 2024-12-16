import { Component } from '@angular/core';
import { ChatComponent } from './components/chat/chat.component';
import { MatIconModule } from '@angular/material/icon';

@Component({
	selector: 'app-root',
	imports: [ChatComponent, MatIconModule],
	templateUrl: './app.component.html',
	styleUrl: './app.component.scss'
})
export class AppComponent {
	isChatOpen: boolean = false;

	constructor() {
	}
}
