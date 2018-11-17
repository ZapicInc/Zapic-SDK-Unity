#import "ZPCShareManager.h"
#import "ZPCLog.h"
#import "ZPCUtils.h"

@interface ZPCShareManager ()
@end

@implementation ZPCShareManager

- (void)share:(ZPCShareMessage *)message {
    NSString *target = message.target;

    UIViewController *viewController = [ZPCUtils getTopViewController];

    if (!target || [target isEqual:@"sheet"]) {
        [self showShareSheet:message onView:viewController];
    } else if ([target isEqual:@"sms"]) {
        if (![MFMessageComposeViewController canSendText]) {
            [ZPCLog error:@"Message services are not available."];
            [self showShareSheet:message onView:viewController];
            return;
        } else {
            MFMessageComposeViewController *composeVC = [[MFMessageComposeViewController alloc] init];
            composeVC.messageComposeDelegate = self;

            NSString *body = message.text;

            if (message.url) {
                body = [body stringByAppendingString:[NSString stringWithFormat:@"\n%@", message.url.absoluteString]];
            }

            if (message.image) {
                //Add the image as attachment
                NSData *dataImg = UIImagePNGRepresentation(message.image);
                [composeVC addAttachmentData:dataImg typeIdentifier:@"public.data" filename:@"Image.png"];
            }

            composeVC.body = body;
            composeVC.subject = message.subject;

            // Present the view controller modally.
            [viewController presentViewController:composeVC animated:YES completion:nil];
        }
    } else if ([target isEqual:@"email"]) {
        if (![MFMailComposeViewController canSendMail]) {
            [ZPCLog error:@"Mail services are not available."];
            [self showShareSheet:message onView:viewController];
            return;
        } else {
            MFMailComposeViewController *composeVC = [[MFMailComposeViewController alloc] init];
            composeVC.mailComposeDelegate = self;

            NSString *body = message.text;

            if (message.url) {
                body = [body stringByAppendingString:[NSString stringWithFormat:@"\n%@", message.url.absoluteString]];
            }

            [composeVC setSubject:message.subject];
            [composeVC setMessageBody:body isHTML:YES];

            // Present the view controller modally.
            [viewController presentViewController:composeVC animated:YES completion:nil];
        }
    }
}

- (void)showShareSheet:(ZPCShareMessage *)message onView:(UIViewController *)viewController {
    NSMutableArray *objectsToShare = [NSMutableArray array];

    if (message.text) {
        [objectsToShare addObject:message.text];
    }

    if (message.image) {
        [objectsToShare addObject:message.image];
    }

    if (message.url) {
        [objectsToShare addObject:message.url];
    }

    UIActivityViewController *shareController = [[UIActivityViewController alloc] initWithActivityItems:objectsToShare applicationActivities:nil];

    [viewController presentViewController:shareController animated:YES completion:nil];
}

- (void)messageComposeViewController:(MFMessageComposeViewController *)controller
                 didFinishWithResult:(MessageComposeResult)result {
    // Check the result or perform other tasks.    // Dismiss the message compose view controller.
    [controller dismissViewControllerAnimated:YES completion:nil];
}

- (void)mailComposeController:(MFMailComposeViewController *)controller
          didFinishWithResult:(MFMailComposeResult)result
                        error:(NSError *)error {
    // Dismiss the mail compose view controller.
    [controller dismissViewControllerAnimated:YES completion:nil];
}

@end
