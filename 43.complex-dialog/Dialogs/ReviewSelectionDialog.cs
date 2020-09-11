// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;

namespace Microsoft.BotBuilderSamples
{
    public class ReviewSelectionDialog : ComponentDialog
    {
        // Define a "done" response for the company selection prompt.
        private const string DoneOption = "done";

        // Define value names for values tracked inside the dialogs.
        private const string answers = "value-companiesSelected";

        // Define the company choices for the company selection prompt.
        private readonly string[] departmentOptions = new string[]
        {
            "Business Solutions", "New Business", "Finance", "TSS", "Software Development", "Sales and Customer Success", "Project Management",
        };
        private readonly string[] happiness = new string[]
        {
            "1","2","3","4","5","6","7","8","9","10"
        };

        private readonly string[] yesNo = new string[]
        {
            "Yes","No"
        };

        private readonly string[] yesNoMaybe = new string[]
        {
            "Yes","No","Maybe"
        };

        private readonly string[] opinion = new string[]
        {
            "Strongly Disagree", "Disagree", "Neutral", "Agree", "Strongly Agree"
        };

        private readonly string[] days = new string[]
        {
            "1","2","3","4","5"
        };

        bool No = false;

        string message = "water";
        public ReviewSelectionDialog()
            : base(nameof(ReviewSelectionDialog))
        {
            // WaterfallStepContext stepContext;
            // CancellationToken cancellationToken;

            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
                {
                    SelectionStepAsync,
                    LoopStepAsync
                    // SelectionStepAsync,
                    // SelectionStepBsync,
                    // SelectionStepCsync,
                    // SelectionStepDsync,
                    // SelectionStepEsync,
                    // SelectionStepFsync,
                    // SelectionStepGsync
                }));

            InitialDialogId = nameof(WaterfallDialog);
        }


        private async Task<DialogTurnResult> SelectionStepAsync(
           WaterfallStepContext stepContext,
           CancellationToken cancellationToken)
        {
            // Continue using the same selection list, if any, from the previous iteration of this dialog.
            var list = stepContext.Options as List<string> ?? new List<string>();
            stepContext.Values[answers] = list;
            var promptOptions = new PromptOptions();
            var options = new List<string>();

            switch (list.Count)
            {

                case 0:

                    message = $"What Department do you work in? *";
                    // Create the list of options to choose from.
                    options = departmentOptions.ToList();

                    break;


                case 1:

                    message = $"on a scale of 1-10, how happy are you? *";
                    // Create the list of options to choose from.
                    options = happiness.ToList();


                    break;

                case 2:

                    message = "Hypothetiacally, if you were to quit tomorrow, what would your reason be?";

                    break;


                case 3:

                    message = $"Do you feel valued at work?*";
                    options = yesNo.ToList();

                    break;

                case 4:
                    if (No is true)
                    {
                        message = "Why?";
                    }
                    break;

                case 5:

                    message = $"Do you see yourself still working here one year from now?*";
                    options = yesNo.ToList();

                    break;

                case 6:

                    message = $"Do you believe the leadership team takes your feedback seriously?*";
                    options = yesNoMaybe.ToList();

                    break;

                case 7:
                    message = $"Do you know the company's values and mission";
                    options = yesNo.ToList();

                    break;

                case 8:
                    message = $"what three words would you use to describe our culture";

                    break;

                case 9:
                    message = "On a scale of 1 to 10, how comfortable do you feel giving feedback to your supervisor?";
                    options = happiness.ToList();

                    break;

                case 10:
                    message = "Information and knowledge are shared openly within the company";
                    options = opinion.ToList();

                    break;

                case 11:
                    message = "My manager always addresses poor performance appropriately";
                    options = opinion.ToList();

                    break;

                case 12:
                    message = "The leaders have communicated a vision that motivates me";
                    options = opinion.ToList();

                    break;

                case 13:

                    message = "I look forward to coming to the office";
                    options = opinion.ToList();

                    break;

                case 14:

                    message = "Do you enjoy working from home? ";
                    options = yesNoMaybe.ToList();

                    break;

                case 15:
                    if (No is true)
                    {
                        message = "Why? ";
                    }
                    break;

                case 16:

                    message = "What challenges do you face working from home?";
                    break;

                case 17:
                    message = $"How many days would you like to work from the office? ";
                    options = days.ToList();

                    break;

                case 18:
                    message = "What do you like about Wragby?";
                    break;

                case 19:
                    message = "What would you like to see differently in H2 FYT?";
                    break;






            }
            promptOptions.Prompt = MessageFactory.Text(message);
            if (options.Count > 0)
            {
                options.Add(DoneOption);
                promptOptions.Choices = ChoiceFactory.ToChoices(options);
                promptOptions.RetryPrompt = MessageFactory.Text("Please choose an option from the list.");
                // Prompt the user for a choice.
                return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
            }
            else
            {
                return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationToken);
            }
        }


        private static async Task<DialogTurnResult> TextStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken, List<string> list, string message)
        {
            // Create an object in which to collect the user's information within the dialog.
            stepContext.Values[answers] = list;

            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text(message) };

            // Ask the user to enter their name.
            return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> LoopStepAsync(
            WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            // Retrieve their selection list, the choice they made, and whether they chose to finish.
            var list = stepContext.Values[answers] as List<string>;
            No = false;
            if (list.Count == 2 || list.Count == 4 || list.Count == 8 || list.Count == 15 || list.Count == 16 || list.Count == 18 || list.Count == 19)
            {
                var choice = (string)stepContext.Result;
                var answer = choice;
                var done = answer == DoneOption;
                if (!done)
                {

                    // If they chose a company, add it to the list.
                    list.Add(answer);
                }
                // if (list.Count == 1)
                // {
                //     TextStepAsync(stepContext, cancellationToken, list, "Hypothetically, if oyu wer to quit tomorrow what would the reason be?");

                // }

                if (done || list.Count > 19)
                {
                    // If they're done, exit and return their list.
                    return await stepContext.EndDialogAsync(list, cancellationToken);
                }
                else
                {
                    // Otherwise, repeat this dialog, passing in the list from this iteration.
                    return await stepContext.ReplaceDialogAsync(nameof(ReviewSelectionDialog), list, cancellationToken);
                }
            }
            else
            {
                var choice = (FoundChoice)stepContext.Result;
                var answer = choice.Value;
                var done = answer == DoneOption;


                if (answer == "No")
                {
                    No = true;
                }
                if ((No is false && list.Count == 3) || (No is false && list.Count == 14))
                {
                    list.Add(answer);
                    list.Add("N/A");
                    return await stepContext.ReplaceDialogAsync(nameof(ReviewSelectionDialog), list, cancellationToken);
                }

                else if (!done)
                {

                    // If they chose a company, add it to the list.
                    list.Add(answer);
                }
                // if (list.Count == 1)
                // {
                //     TextStepAsync(stepContext, cancellationToken, list, "Hypothetically, if oyu wer to quit tomorrow what would the reason be?");

                // }

                if (done || list.Count > 19)
                {
                    // If they're done, exit and return their list.
                    return await stepContext.EndDialogAsync(list, cancellationToken);
                }
                else
                {
                    // Otherwise, repeat this dialog, passing in the list from this iteration.
                    return await stepContext.ReplaceDialogAsync(nameof(ReviewSelectionDialog), list, cancellationToken);
                }
            }






        }

    }
}
